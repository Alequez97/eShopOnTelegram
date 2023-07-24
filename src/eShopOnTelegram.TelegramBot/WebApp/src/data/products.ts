import Product from "../types/Product";

export function getProducts(): Product[] {
  return [
    {
      id: 1,
      name: "Banana",
      productCategoryName: "E-Cigarette",
      originalPrice: 7.99,
      quantityLeft: 10,
      image:
        "https://royal-mmxxi.com/wp-content/uploads/2021/03/600-pink-lemonade-web-300x300.jpg",
    },
    {
      id: 2,
      name: "Grape",
      productCategoryName: "E-Cigarette",
      originalPrice: 7.99,
      quantityLeft: 10,
      image:
        "https://royal-mmxxi.com/wp-content/uploads/2021/03/600-grape-ice-web-600x600.jpg",
    },
    {
      id: 3,
      name: "Lychee",
      productCategoryName: "E-Cigarette",
      originalPrice: 7.99,
      quantityLeft: 10,
      image:
        "https://royal-mmxxi.com/wp-content/uploads/2021/04/600-lychee-ice-web-300x300.jpg",
    },
    {
      id: 4,
      name: "Strawberry",
      productCategoryName: "E-Cigarette",
      originalPrice: 7.99,
      quantityLeft: 10,
      image:
        "https://royal-mmxxi.com/wp-content/uploads/2021/04/600-strawberry-and-watermelon-ice-web-600x600.jpg",
    },
  ];
}
