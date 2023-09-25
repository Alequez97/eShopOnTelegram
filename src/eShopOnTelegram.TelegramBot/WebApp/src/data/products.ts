import { Product } from "../types/product";

export function getProducts(): Product[] {
  return [
    {
      id: 1,
      name: "JS Never Smokes",
      productCategoryName: "E-Cigarette",
      productAttributes: [
        {
          id: 1,
          color: "Pink",
          originalPrice: 9.99,
          quantityLeft: 100,
          image:
            "https://royal-mmxxi.com/wp-content/uploads/2021/03/600-pink-lemonade-web-300x300.jpg",
        },
        {
          id: 2,
          color: "Grape",
          originalPrice: 9.99,
          priceWithDiscount: 7.99,
          quantityLeft: 100,
          image:
            "https://royal-mmxxi.com/wp-content/uploads/2021/03/600-grape-ice-web-600x600.jpg",
        },
        {
          id: 3,
          color: "Lychee",
          originalPrice: 9.99,
          priceWithDiscount: 7.99,
          quantityLeft: 100,
          image:
            "https://royal-mmxxi.com/wp-content/uploads/2021/04/600-lychee-ice-web-300x300.jpg",
        },
        {
          id: 4,
          color: "Strawberry",
          originalPrice: 9.99,
          priceWithDiscount: 7.99,
          quantityLeft: 100,
          image:
            "https://royal-mmxxi.com/wp-content/uploads/2021/04/600-strawberry-and-watermelon-ice-web-600x600.jpg",
        },
      ],
    },
  ];
}
