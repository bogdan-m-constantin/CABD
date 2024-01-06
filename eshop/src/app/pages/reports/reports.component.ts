import { Component, OnInit } from '@angular/core';
import { Card, EvolutieCard, IstoricCard } from '../../domain/domain';
import { WebApiService } from '../../services/WebApi.service';

@Component({
  selector: 'app-reports',
  templateUrl: './reports.component.html',
  styleUrl: './reports.component.scss'
})
export class ReportsComponent implements OnInit {

  carduri: Card[] = []
  card1?: Card
  card2?: Card
  card3?: Card
  card4?: Card
  timestamp?: Date
  raport1?: Card;
  raport2?: IstoricCard;
  raport3?: EvolutieCard[];
  raport4?: Card;
  minut: number = 10;
  ora: number = 10;
  secunda: number = 10;
  constructor(private api: WebApiService) {

  }
  ngOnInit(): void {
    this.api.getCards(-1).then(e => { this.carduri = e; this.card1 = e[0]; this.card2 = e[0]; this.card3 = e[0]; this.card4 = e[0] });
  }
  async showReport1() {
    if (this.card1)
      this.raport1 = await this.api.getCard(this.card1!.serie)
  }
  async showReport2() {

    if (this.card2)
      this.raport2 = await this.api.getIntervalMax(this.card2!.serie)
  }
  async showReport3() {

    if (this.card3)
      this.raport3 = await this.api.getIstoricCard(this.card3!.serie)
  }
  async showReport4() {
    if (this.card4 && this.timestamp) {
      this.timestamp.setHours(this.ora);
      this.timestamp.setMinutes(this.minut);
      this.timestamp.setSeconds(this.secunda);
      this.raport4 = await this.api.getCardT(this.card4!.serie, this.timestamp)
    }

  }
}
