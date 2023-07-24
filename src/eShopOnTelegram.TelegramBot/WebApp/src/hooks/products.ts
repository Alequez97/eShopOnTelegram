import axios, { AxiosError } from "axios";
import { useEffect, useState } from "react";
import Product from "../types/Product";

export function useProducts() {
  const [products, setProducts] = useState<Product[]>([]);
  const [productCategories, setProductCategories] = useState<string[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");

  async function fetchProducts() {
    try {
      setLoading(true);
      const response = await axios.get<Product[]>("/products");
      setProducts(response.data);
      setLoading(false);
    } catch (e: unknown) {
      const error = e as AxiosError;
      setLoading(false);
      setError(error.message);
    }
  }

  useEffect(() => {
    fetchProducts();
  }, []);

  useEffect(() => {
    setProductCategories(
      products
        .map((product) => product.productCategoryName)
        .filter(
          (categoryName) =>
            categoryName !== undefined &&
            categoryName !== null &&
            categoryName !== ""
        )
        .filter((value, index, array) => array.indexOf(value) === index)
    );
  }, [products]);

  return { products, productCategories, error, loading };
}
