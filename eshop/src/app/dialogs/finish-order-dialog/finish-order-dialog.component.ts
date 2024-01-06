import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Adresa, Card, Comanda } from '../../domain/domain';
import { WebApiService } from '../../services/WebApi.service';

@Component({
  selector: 'app-finish-order-dialog',
  templateUrl: './finish-order-dialog.component.html',
  styleUrl: './finish-order-dialog.component.scss'
})
export class FinishOrderDialogComponent implements OnInit {
  selectedCard: Card | null = null;
  cards: Card[] = []
  adrese: Adresa[] = []
  total: number = 0;
  grandTotal: number = 0;
  constructor(private dialogRef: MatDialogRef<FinishOrderDialogComponent>, @Inject(MAT_DIALOG_DATA) public comanda: Comanda, private api: WebApiService) {

  }
  async ngOnInit() {
    this.cards = await this.api.getCards(this.comanda.client.id)
    this.adrese = await this.api.getAddresses(this.comanda.client.id);
    this.total = this.comanda.pozitii.reduce((sum, current) => sum + current.pret * current.cantitate, 0);
    this.deductPoints(null);
  }
  deductPoints(card: Card | null) {
    this.comanda.puncteFolosite = Math.min(card?.puncte ?? 0, Math.ceil(this.total * 100))
    this.grandTotal = this.total - this.comanda.puncteFolosite / 100.00
  }
  async finishOrder() {

    if (this.comanda.adresaFacturare != null && this.comanda.adresaLivrare != null) {
      this.comanda.dataOra = new Date().toISOString();
      await this.api.insertOrder(this.comanda);
      this.dialogRef.close(this.comanda);
      this.total = 0;
    }
    else {
      alert("The adresses are required");
    }

  }
}
