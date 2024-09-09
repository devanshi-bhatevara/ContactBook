import { Country } from "./country.model"
import { State } from "./state.model"

export interface AddContact {
    firstName: string,
    lastName: string,
    phone: string,
    address: string,
    email: string,
    gender: string,
    isFavourite: boolean,
    countryId: number,
    stateId: number,
    fileName: null,
    imageByte:string,
    country: Country,
    state: State,
    birthDate: Date | null 

}