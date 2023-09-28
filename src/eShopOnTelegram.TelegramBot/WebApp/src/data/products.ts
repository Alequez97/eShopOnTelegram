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
          originalPrice: 8.99,
          priceWithDiscount: 5.99,
          quantityLeft: 0,
          image:
            "https://royal-mmxxi.com/wp-content/uploads/2021/03/600-grape-ice-web-600x600.jpg",
        },
        {
          id: 3,
          color: "Lychee",
          originalPrice: 7.99,
          priceWithDiscount: 5.99,
          quantityLeft: 10,
          image:
            "https://royal-mmxxi.com/wp-content/uploads/2021/04/600-lychee-ice-web-300x300.jpg",
        },
        {
          id: 4,
          color: "Strawberry",
          originalPrice: 10.99,
          priceWithDiscount: 7.99,
          quantityLeft: 5,
          image:
            "https://royal-mmxxi.com/wp-content/uploads/2021/04/600-strawberry-and-watermelon-ice-web-600x600.jpg",
        },
      ],
    },
    {
      id: 2,
      name: "JS Never Smokes",
      productCategoryName: "E-Cigarette",
      productAttributes: [
        {
          id: 5,
          color: "Pink",
          size: "M",
          originalPrice: 9.99,
          quantityLeft: 100,
          image:
            "https://royal-mmxxi.com/wp-content/uploads/2021/03/600-pink-lemonade-web-300x300.jpg",
        },
        {
          id: 6,
          color: "Grape",
          size: "M",
          originalPrice: 8.99,
          priceWithDiscount: 5.99,
          quantityLeft: 100,
          image:
            "https://royal-mmxxi.com/wp-content/uploads/2021/03/600-grape-ice-web-600x600.jpg",
        },
        {
          id: 7,
          color: "Lychee",
          size: "L",
          originalPrice: 7.99,
          priceWithDiscount: 5.99,
          quantityLeft: 10,
          image:
            "https://royal-mmxxi.com/wp-content/uploads/2021/04/600-lychee-ice-web-300x300.jpg",
        },
        {
          id: 8,
          color: "Strawberry",
          size: "S",
          originalPrice: 10.99,
          priceWithDiscount: 7.99,
          quantityLeft: 5,
          image:
            "https://royal-mmxxi.com/wp-content/uploads/2021/04/600-strawberry-and-watermelon-ice-web-600x600.jpg",
        },
      ],
    },
  ];
}
