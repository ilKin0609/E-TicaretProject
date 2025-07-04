namespace E_Ticaret_Project.Application.Shared.Permissions;

public static class Permission
{
    public static class Category
    {
        public const string Create = "Category.Create";
        public const string Update = "Category.Update";
        public const string Delete = "Category.Delete";

        public static List<string> All = new()
        {
            Create,
            Update,
            Delete
        };
    }

    public static class Favorite
    {
        public const string Create = "Favorite.Create";
        public const string Update = "Favorite.Update";
        public const string Delete = "Favorite.Delete";

        public static List<string> All = new()
        {
            Create,
            Update,
            Delete
        };
    }
    public static class Image
    {
        public const string Create = "Image.Create";
        public const string Update = "Image.Update";
        public const string Delete = "Image.Delete";

        public static List<string> All = new()
        {
            Create,
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
        public const string GetMySales = "Order.Delete";

        public static List<string> All = new()
        {
            Create,
            Delete,
            Update,
            GetMy,
            GetAll,
            GetMySales
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

        public static List<string> All = new()
        {
            Create,
            Update,
            Delete,
            GetMy,
            DeleteProductImage,
            AddProductImage
        };
    }
    public static class ReviewAndComment
    {
        public const string Create = "ReviewAndComment.Create";
        public const string Update = "ReviewAndComment.Update";
        public const string Delete = "ReviewAndComment.Delete";

        public static List<string> All = new()
        {
            Create,
            Update,
            Delete
        };
    }
    public static class Role
    {
        public const string GetAllPermissions = "Role.GetAllPermissions";
        public const string Create = "Role.Create";
        public const string Update = "Role.Update";

        public static List<string> All = new()
        {
            GetAllPermissions,
            Create,
            Update
        };
    }
    public static class Account
    {
        public const string AddRole = "Account.AddRole";


        public static List<string> All = new()
        {
           AddRole
        };
    }
}
