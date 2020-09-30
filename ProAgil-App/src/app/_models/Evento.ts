import { Lote } from './Lote';
import { Palestrante } from './Palestrante';
import { RedeSocial } from './RedeSocial';

export class Evento {
    constructor() {}

    id: number;
    local: string;
    dataEvento: Date    ;
    tema: string;
    qtdPessoas: number;
    lotes: Lote[];
    telefone: string;
    email: string;
    imagemURL: string;
    redesSociais: RedeSocial[];
    palestrantesEventos: Palestrante[];
}
