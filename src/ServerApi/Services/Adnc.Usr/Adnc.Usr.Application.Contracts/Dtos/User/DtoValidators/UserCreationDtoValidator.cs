﻿using Adnc.Shared.Consts.Entity.Usr;
using Adnc.Usr.Application.Contracts.Dtos;
using FluentValidation;

namespace Adnc.Usr.Application.Contracts.DtoValidators
{
    public class UserCreationDtoValidator : AbstractValidator<UserCreationDto>
    {
        public UserCreationDtoValidator()
        {
            Include(new UserCreationAndUpdationDtoValidator());
            RuleFor(x => x.Password).NotEmpty().Length(5, UserConsts.Password_Maxlength);
            //RuleFor(x => x.Password).NotEmpty().When(x => x.Id < 1)
            //                        .Length(5, 16).When(x => x.Id < 1);
        }
    }
}