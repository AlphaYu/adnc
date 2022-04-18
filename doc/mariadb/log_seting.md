```bash
# 查看日志是否开启
show variables like 'general_log';
# 查看日志输出类型  table或file
show variables like 'log_output';
# 查看日志文件保存位置
show variables like 'general_log_file';
# 设置日志文件保存位置
set global general_log_file='/tmp/general_log';
# 开启日志功能
set global general_log=on;
# 设置输出类型为 table
set global log_output='table';
# 设置输出类型为file
set global log_output='file';
#清空mysql默认数据库 genral_log 表数据
truncate table mysql.general_log; 
#转移表数据 genral_log->genral_log_bak
create table mysql.general_log_bak as select * from mysql.;
#查询日志
SELECT * FROM mysql.general_log where user_host='root[root] @  [223.73.185.137]' order by event_time desc  limit 0,100 
```