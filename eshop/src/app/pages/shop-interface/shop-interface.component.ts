import { Component, OnInit } from '@angular/core';
import { Produs, Comanda, Client } from '../../domain/domain';
import { WebApiService } from '../../services/WebApi.service';
import { MatDialog } from '@angular/material/dialog';
import { FinishOrderDialogComponent } from '../../dialogs/finish-order-dialog/finish-order-dialog.component';

@Component({
  selector: 'app-shop-interface',
  templateUrl: './shop-interface.component.html',
  styleUrl: './shop-interface.component.scss'
})
export class ShopInterfaceComponent implements OnInit {
  produse: Produs[] = []
  clienti: Client[] = []
  visibleProducts: Produs[][] = [];
  total: number = 0;
  comanda: Comanda = {
    id: -1,
    client: {
      id: 1,
      nume: "",
      prenume: "",
      dataNasterii: "2000-01-01",
      adrese: [],
      email: "",
      telefon: ""
    },
    dataOra: new Date().toISOString(),
    pozitii: [],
    puncteFolosite: 0,
  }
  constructor(private api: WebApiService, private dialog: MatDialog) {

  }
  ngOnInit(): void {

    this.comanda = JSON.parse(window.localStorage.getItem("comanda") ?? JSON.stringify(this.comanda))

    this.total = this.comanda.pozitii.reduce((sum, current) => sum + current.pret * current.cantitate, 0);
    this.loadData().catch((e) => console.error(e))

  }
  async loadData() {
    this.produse = await this.api.getProduse();
    this.clienti = await this.api.getClienti();
    this.comanda.client = JSON.parse(window.localStorage.getItem("client") ?? JSON.stringify(this.clienti[0]))
    console.log(this.produse);

  }
  addItemToCart(produs: Produs) {
    let index = this.comanda.pozitii.findIndex(e => e.produs.id == produs.id)
    if (index == -1)
      this.comanda.pozitii.push({
        id: -1,
        cantitate: 1,
        pret: produs.pret,
        tva: produs.tva,
        puncte: Math.floor(produs.pret),
        produs
      })
    else {
      this.comanda.pozitii[index].cantitate++;
    }
    this.total = this.comanda.pozitii.reduce((sum, current) => sum + current.pret * current.cantitate, 0);
    window.localStorage.setItem("comanda", JSON.stringify(this.comanda));
  }
  finishOrder() {
    const ref = this.dialog.open(FinishOrderDialogComponent, {
      width: '80vw',
      height: '80vh',
      data: this.comanda
    })
    ref.afterClosed().subscribe(e => {
      if (e) {
        this.comanda = {
          id: -1,
          client: {
            id: 1,
            nume: "",
            prenume: "",
            dataNasterii: "2000-01-01",
            adrese: [],
            email: "",
            telefon: ""
          },
          dataOra: new Date().toISOString(),
          pozitii: [],
          puncteFolosite: 0,
        }
        window.localStorage.setItem("comanda", JSON.stringify(this.comanda));
      }
    })
  }
  setClient(client: Client) {
    this.comanda.client = client;
    window.localStorage.setItem("client", JSON.stringify(this.comanda.client));

  }


}
