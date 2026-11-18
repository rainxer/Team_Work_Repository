using Microsoft.AspNetCore.Mvc;
using WikipediaApi.Data;
using WikipediaApi.Models;

namespace WikipediaApi.Controllers;

public class RolesController : CrudController<Role>
{
    public RolesController(WikipediaContext db)
        : base(db, db.Roles) { }
}

public class CategoriesController : CrudController<Category>
{
    public CategoriesController(WikipediaContext db)
        : base(db, db.Categories) { }
}

public class ManufacturersController : CrudController<Manufacturer>
{
    public ManufacturersController(WikipediaContext db)
        : base(db, db.Manufacturers) { }
}

public class CharacteristicsController : CrudController<Characteristic>
{
    public CharacteristicsController(WikipediaContext db)
        : base(db, db.Characteristics) { }
}

public class StoresController : CrudController<Store>
{
    public StoresController(WikipediaContext db)
        : base(db, db.Stores) { }
}

public class ProductsController : CrudController<Product>
{
    public ProductsController(WikipediaContext db)
        : base(db, db.Products) { }
}

public class PricesController : CrudController<Price>
{
    public PricesController(WikipediaContext db)
        : base(db, db.Prices) { }
}

public class ReviewsController : CrudController<Review>
{
    public ReviewsController(WikipediaContext db)
        : base(db, db.Reviews) { }
}

public class FavoritesController : CrudController<Favorite>
{
    public FavoritesController(WikipediaContext db)
        : base(db, db.Favorites) { }
}