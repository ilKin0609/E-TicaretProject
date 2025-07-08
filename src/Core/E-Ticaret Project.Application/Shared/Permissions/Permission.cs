namespace E_Ticaret_Project.Application.Shared.Permissions;

public static class Permission
{
    public static class Category
    {
        public const string MainCreate = "Category.MainCreate";
        public const string SubCreate = "Category.SubCreate";
        public const string Update = "Category.Update";
        public const string Delete = "Category.Delete";

        public static List<string> All = new()
        {
            MainCreate,
            SubCreate,
            Update,
            Delete
        };
    }

    public static class Order
    {
        public const string Create = "Order.Create";
        public const string Delete = "Order.Delete";
        public const string Update = "Order.Update";
        public const string GetMy = "Order.GetMy";
        public const string GetAll = "Order.GetAll";
        public const string GetDetail = "Order.GetDetail";
        public const string GetMySales = "Order.MySales";

        public static List<string> All = new()
        {
            Create,
            Delete,
            Update,
            GetMy,
            GetAll,
            GetMySales,
            GetDetail
        };
    }
    public static class Product
    {
        public const string Create = "Product.Create";
        public const string Update = "Product.Update";
        public const string Delete = "Product.Delete";
        public const string GetMy = "Product.GetMy";
        public const string DeleteProductImage = "Product.DeleteProductImage";
        public const string AddProductImage = "Product.AddProductImage";
        public const string AddProductFavorite = "Product.AddProductFavorite";
        public const string DeleteProductFavorite = "Product.DeleteProductFavorite";

        public static List<string> All = new()
        {
            Create,
            Update,
            Delete,
            GetMy,
            DeleteProductImage,
            AddProductImage,
            AddProductFavorite,
            DeleteProductFavorite
        };
    }
    public static class ReviewAndComment
    {
        public const string Create = "ReviewAndComment.Create";
        public const string Delete = "ReviewAndComment.Delete";

        public static List<string> All = new()
        {
            Create,
            Delete
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
    public static class Account
    {
        public const string AddRole = "Account.AddRole";
        public const string Create = "Account.Create";
        public const string GetAllUser = "Account.GetAllUser";
        public const string GetUser = "Account.GetUser";


        public static List<string> All = new()
        {
           AddRole,
           Create,
           GetAllUser,
           GetUser
        };
    }

    public static class User
    {
        public const string ResetPassword = "User.ResetPassword";
        public const string GetMy = "User.GetMy";


        public static List<string> All = new()
        {
           ResetPassword,
           GetMy
        };
    }
}
