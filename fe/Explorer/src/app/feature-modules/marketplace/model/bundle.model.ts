import { Product } from "./product.model";

export interface Bundle{
    id: number,
    name: string,
    price: number,
    status: number,
    creatorId: number,
    products: Product[]
}