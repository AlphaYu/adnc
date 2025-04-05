@echo off
title Encoding Converter

REM 指定的一种文件类型（只支持一类）
set fileType=*.cs

REM 是否遍历子目录：需要遍历则设置为：-Recurse，否则置为空
REM set isRecurse=
set isRecurse=-Recurse

REM 转换前的文件编码（可以随便填一个，程序自动判断）
set fromEnc=GBK

REM 转换后的文件编码
set toEnc=UTF-8

REM 转换后的文件名前缀，默认与转换的文件在同一目录，可以为空（如果为空则直接覆盖原文件）
REM set prefix=
set prefix=

powershell.exe -command "$files = dir %fileType% %isRecurse%; $prefix = '%prefix%'; $from = '%fromEnc%'; $to = '%toEnc%'; $e = [System.Text.Encoding]; $sr = [System.IO.StreamReader]; $sw = [System.IO.StreamWriter]; $isSame=[string]::IsNullOrEmpty($prefix); function getEnc($encName){if('UTF-8' -eq $encName.ToUpper()){return [System.Text.UTF8Encoding]::new($false);}return [System.Text.Encoding]::GetEncoding($encName); }; function getFileEnc($data) {[object[]]$res = @([System.Text.Encoding]::Default,$false);if($data[0] -eq 0x2B -and $data[1] -eq 0x2F -and $data[2] -eq 0x76) { return @([System.Text.Encoding]::UTF7,$true);}elseif($data[0] -eq 0xEF -and $data[1] -eq 0xBB -and $data[2] -eq 0xBF) { return @([System.Text.Encoding]::UTF8,$true);}elseif($data[0] -eq 0xFF -and $data[1] -eq 0xFE) { return @([System.Text.Encoding]::Unicode,$true);}elseif($data[0] -eq 0xFE -and $data[1] -eq 0xFF) { return @([System.Text.Encoding]::BigEndianUnicode,$true);}elseif($data[0] -eq 0 -and $data[1] -eq 0 -and $data[2] -eq 0xFE -and $data[3] -eq 0xFF) { return @([System.Text.Encoding]::UTF32,$true);}else{ [int]$charByteCounter = 1; [byte]$curByte = 0; for ($i = 0; $i -lt $data.Length; $i++) {$curByte = $data[$i];if ($charByteCounter -eq 1){if ($curByte -ge 0x80){ while (( ($curByte = ($curByte -shl 1)) -band 0x80) -ne 0) {$charByteCounter++; } if ($charByteCounter -eq 1 -or $charByteCounter -gt 6) {return $res; }}}else{if (($curByte -band 0xC0) -ne 0x80){ return $res;}$charByteCounter--;} } if ($charByteCounter -gt 1) {return $res; } return @([System.Text.Encoding]::UTF8,$false);}}; if($files.Count -lt 1){Write-Host 'No Files...';exit; }; Write-Host ('Begin:'+(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')); foreach($file in $files){ $newFile = (Join-Path -Path $file.DirectoryName -ChildPath ($prefix+$file.BaseName+$file.Extension)); if($isSame){ $newFile = (Join-Path -Path $file.DirectoryName -ChildPath ('TEMP_'+$file.BaseName+$file.Extension));}; $fromEnc = (getEnc -encName $from); $toEnc = (getEnc -encName $to); $fileEnc = (getFileEnc -data ([System.IO.File]::ReadAllBytes($file.FullName))); if($fileEnc[0].BodyName -eq $toEnc.BodyName -and $fileEnc[1] -eq $false){   Write-Host ($file.FullName+',no need to convert.');  continue; } if($fromEnc -ne $fileEnc[0]){ Write-Host ($file.FullName+',encoding should be:' + $fileEnc[0].BodyName); $fromEnc = (getEnc -encName $fileEnc[0].BodyName); } $srIn = $sr::new($file.FullName,$fromEnc); $swOut = $sw::new($newFile,$false,$toEnc,4096); while(($line = $srIn.ReadLine()) -ne $null){$swOut.WriteLine($line);} $srIn.Close();$swOut.Flush();$swOut.Close(); if($isSame){Move-Item -Path $newFile -Destination $file -Force};}; Write-Host ('Files:'+$files.Count+',success...'); Write-Host ('End:'+(Get-Date -Format 'yyyy-MM-dd HH:mm:ss'));"
pause
