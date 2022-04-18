﻿using Adnc.Shared.Consts.Entity.Usr;
using Adnc.Usr.Application.Contracts.Dtos;
using FluentValidation;

namespace Adnc.Usr.Application.Contracts.DtoValidators
{
    public class UserLoginDtoValidator : AbstractValidator<UserLoginDto>
    {
        public UserLoginDtoValidator()
        {
            RuleFor(x => x.Account).NotEmpty().Length(5, UserConsts.Account_MaxLength);
            RuleFor(x => x.Password).NotEmpty().Length(5, UserConsts.Password_Maxlength);
        }
    }
}