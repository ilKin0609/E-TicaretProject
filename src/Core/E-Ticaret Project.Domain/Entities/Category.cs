﻿namespace E_Ticaret_Project.Domain.Entities;

public class Category : BaseEntity
{
    public string Name { get; set; }

    public Guid? ParentCategoryId { get; set; }
    public Category? ParentCategory { get; set; }
    public ICollection<Category> SubCategories { get; set; }


    public ICollection<Product> Products {get; set;}=new List<Product>();

}
