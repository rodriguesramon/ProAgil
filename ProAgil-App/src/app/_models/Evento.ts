import { Lote } from './Lote';
import { Palestrante } from './Palestrante';
import { RedeSocial } from './RedeSocial';

export interface Evento {
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
