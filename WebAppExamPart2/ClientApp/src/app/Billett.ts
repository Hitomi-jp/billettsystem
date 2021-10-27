export class Billett {
  id?: number;
  kundeId: number;
  destinationFrom: string;
  destinationTo: string;
  ticketType: string;
  lugarType: string;
  departureDato: string;
  returnDato: string;
  antallAdult: number;
  antallChild: number;
  pris: number;
}
