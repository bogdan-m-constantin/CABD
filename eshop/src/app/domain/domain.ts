
export interface Adresa {
  id: number,
  denumire: string,
  adresaIntreaga: string,
  judet: string,
  localitate: string,
  codFiscal: string,
  nrRegCom: string
}
export interface Card {
  serie: string,
  puncte: number,
}

export interface IstoricCard {
  serie: string,
  puncte: string
  start: string,
  end: string,
}
export interface EvolutieCard {
  timestamp: string,
  puncte: number,
  diferenta: number,
}
export interface Client {
  id: number,
  nume?: string,
  prenume?: string,
  email?: string,
  dataNasterii?: string,
  telefon?: string,
  adrese?: Adresa[]
}
export interface Comanda {
  id: number,
  client: Client,
  adresaLivrare?: Adresa,
  adresaFacturare?: Adresa,
  dataOra?: string,
  card?: Card | null,
  pozitii: PozitieComanda[],
  puncteFolosite: number,
}
export interface PozitieComanda {
  id: number,
  produs: Produs,
  cantitate: number,
  pret: number,
  tva: number,
  puncte: number
}
export interface Produs {
  id: number,
  denumire: string,
  tva: number,
  pret: number,
}
