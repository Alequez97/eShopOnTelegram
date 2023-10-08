export const replaceEmptyKeysWithNull = async (obj: any) => {
    for (const key in obj) {
        if (typeof obj[key] === "string" && obj[key].trim() === "") {
            obj[key] = null;
        }
    }
}