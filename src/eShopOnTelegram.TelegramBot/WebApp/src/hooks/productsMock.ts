import { AxiosError } from "axios";
import { useEffect, useState } from "react";
import { getProducts } from "../data/products";
import Product from "../types/product";

export function useProductsMock() {
  const [products, setProducts] = useState<Product[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");

  async function fetchProducts() {
    try {
      setLoading(true);
      const products = getProducts();
      setProducts(products);
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

  return { products, error, loading };
}
