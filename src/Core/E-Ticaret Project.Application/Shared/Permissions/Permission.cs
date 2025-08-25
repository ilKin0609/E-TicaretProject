using Microsoft.AspNetCore.Http.HttpResults;

namespace E_Ticaret_Project.Application.Shared.Permissions;

public static class Permission
{
    public static class Category
    {
        public const string Create = "Category.Create";
        public const string Update = "Category.Update";
        public const string Delete = "Category.Delete";
        public const string Toggle = "Category.Toggle";
        public const string Reorder = "Category.Reorder";
        public const string ReorderList = "Category.ReorderList";
        public const string ChangeParent = "Category.ChangeParent";

        public static List<string> All = new()
        {
            Create,
            Update,
            Delete,
            Toggle,
            Reorder,
            ReorderList,
            ChangeParent,
        };
    }
    public static class Product
    {
        public const string Create = "Product.Create";
        public const string Update = "Product.Update";
        public const string Delete = "Product.Delete";
        public const string UploadMainImage = "Product.UploadMainImage";
        public const string UploadAdditionalImage = "Product.UploadAdditionalImage";
        public const string RemoveImage = "Product.RemoveImage";
        public const string SetMain = "Product.SetMain";
        public const string ReorderImage = "Product.ReorderImage";
        public const string UpdateImageAlt = "Product.UpdateImageAlt";

        public static List<string> All = new()
        {
            Create,
            Update,
            Delete,
            UploadMainImage,
            UploadAdditionalImage,
            RemoveImage,
            SetMain,
            ReorderImage,
            UpdateImageAlt
        };
    }

    public static class ContactInfo
    {
        public const string Update = "Contact.Update";

        public static List<string> All = new()
        {
            Update
        };
    }

    public static class About
    {
        public const string Update = "About.Update";
        public const string UploadImage = "About.UploadImage";
        public const string RemoveImage = "About.RemoveImage";

        public static List<string> All = new()
        {
            
            Update,
            UploadImage,
            RemoveImage
            
        };
    }

    public static class Role
    {
        public const string GetAllPermissions = "Role.GetAllPermissions";
        public const string Create = "Role.Create";
        public const string Update = "Role.Update";
        public const string Delete = "Role.Delete";

        public static List<string> All = new()
        {
            GetAllPermissions,
            Create,
            Update,
            Delete
        };
    }
    public static class Admin
    {
        public const string AddRole = "Admin.AddRole";
        public const string Create = "Admin.CreateUser";
        public const string GetAllUser = "Admin.GetAllUser";
        public const string Delete = "Admin.DeleteUser";
        public const string Update = "Admin.UpdateUser";
        public const string Toggle = "Admin.ToggleUser";
        public const string UnToggle = "Admin.UnToggleUser";


        public static List<string> All = new()
        {
           AddRole,
           Create,
           GetAllUser,
           Delete,
           Update,
           Toggle,
           UnToggle
        };
    }

    public static class SiteSetting
    {
        public const string Update = "SiteSetting.Update";


        public static List<string> All = new()
        {
           Update
        };
    }

    public static class InstaLink
    {
        public const string Create = "InstaLink.Create";
        public const string Delete = "InstaLink.Delete";


        public static List<string> All = new()
        {
           Create,
           Delete
        };
    }
}
