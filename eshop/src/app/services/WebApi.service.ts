import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Adresa, Card, Client, Comanda, EvolutieCard, IstoricCard, Produs } from '../domain/domain';
import { firstValueFrom } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class WebApiService {
  baseUrl = "http://localhost:5072"

  constructor(private http: HttpClient) { }

  getProduse(): Promise<Produs[]> {
    return firstValueFrom(this.http.get<Produs[]>(`${this.baseUrl}/produse`))
  }
  getCards(client: number): Promise<Card[]> {
    return firstValueFrom(this.http.get<Card[]>(`${this.baseUrl}/carduri/client/${client}`))
  }
  getClienti(): Promise<Client[]> {
    return firstValueFrom(this.http.get<Client[]>(`${this.baseUrl}/clienti`))
  }
  getAddresses(client: number): Promise<Adresa[]> {
    return firstValueFrom(this.http.get<Adresa[]>(`${this.baseUrl}/adrese/${client}`))
  }
  insertOrder(comanda: Comanda): Promise<Comanda> {
    return firstValueFrom(this.http.post<Comanda>(`${this.baseUrl}/comenzi`, comanda))
  }
  getIstoricCard(serie: string): Promise<EvolutieCard[]> {
    return firstValueFrom(this.http.get<EvolutieCard[]>(`${this.baseUrl}/carduri/istoric/${serie}`))
  }

  getIntervalMax(serie: string): Promise<IstoricCard> {
    return firstValueFrom(this.http.get<IstoricCard>(`${this.baseUrl}/carduri/interval-max/${serie}`))
  }

  getCard(serie: string): Promise<Card> {
    return firstValueFrom(this.http.get<Card>(`${this.baseUrl}/carduri/serie/${serie}`))
  }
  getCardT(serie: string, timestamp: Date): Promise<Card> {
    return firstValueFrom(this.http.get<Card>(`${this.baseUrl}/carduri/stare/${serie}/${timestamp.toISOString()}`))
  }

}
