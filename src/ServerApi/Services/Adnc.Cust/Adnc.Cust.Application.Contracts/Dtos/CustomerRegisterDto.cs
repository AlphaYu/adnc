﻿namespace Adnc.Cust.Application.Contracts.Dtos;

public class CustomerRegisterDto : InputDto
{
    public string Account { get; set; }

    public string Password { get; set; }

    public string Nickname { get; set; }

    public string Realname { get; set; }
}