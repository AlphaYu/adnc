namespace Adnc.Demo.Const.Entity.Admin;

public static class UserConsts
{
    public const int Account_MaxLength = 32;
    public const int Avatar_MaxLength = 128;
    public const int Email_Maxlength = 32;
    public const int Name_Maxlength = 32;
    public const int Password_Maxlength = 32;
    public const int Phone_Maxlength = 11;
    public const int RoleIds_Maxlength = 72;
    public const int Salt_Maxlength = 6;
}

public static class RoleConsts
{
    public const int Name_MaxLength = 32;
    public const int Code_MaxLength = 32;
}

public static class DeptConsts
{
    public const int Name_MaxLength = 32;
    public const int Code_MaxLength = 16;
    public const int Pids_MaxLength = 128;
}

public static class MenuConsts
{
    public const int Code_MaxLength = 32;
    public const int ParentIds_MaxLength = 128;
    public const int Component_MaxLength = 64;
    public const int Icon_MaxLength = 32;
    public const int Name_MaxLength = 32;
    public const int RouteName_MaxLength = 64;
    public const int RoutePath_MaxLength = 64;
    public const int Redirect_MaxLength = 128;
    public const int Title_MaxLength = 16;
    public const int Type_MaxLength = 16;
    public const int Params_MaxLength = 128;
}

public static class SysConfigConsts
{
    public const int Key_MaxLength = 64;
    public const int Name_MaxLength = 64;
    public const int Value_MaxLength = 128;
    public const int Remark_MaxLength = 128;
}

public static class DictConsts
{
    public const int Code_MaxLength = 32;
    public const int Name_MaxLength = 32;
    public const int Remark_MaxLength = 128;
}

public static class DictDataConsts
{
    public const int Label_MaxLength = 32;
    public const int Value_MaxLength = 32;
    public const int TagType_MaxLength = 32;
}

public static class NoticeConsts
{
    public const int Title_MaxLength = 64;
    public const int Type = 16;
    public const int TargetType = 16;
    public const int Content_MaxLength = 255;
}
