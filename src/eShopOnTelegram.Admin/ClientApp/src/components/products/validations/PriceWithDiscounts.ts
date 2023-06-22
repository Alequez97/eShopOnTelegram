export const shouldBeLessThanOriginalPrice = (value: any, allValues: any) => {
  if (value >= allValues.originalPrice) {
    return "Discounted price should be less than the original price";
  }
  return undefined; // Return undefined if the validation passes
};
