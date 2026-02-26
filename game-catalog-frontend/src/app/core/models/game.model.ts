/**
 * The primary interface representing a Video Game from the database.
 */
export interface Game {
  id: string;        
  name: string;         
  description: string;   
  price: number;     
  genre: string;      
  gameImageURL: string|null;  
}


export type CreateGameDto = Omit<Game, 'id'>;


export interface UpdateGameDto extends Partial<CreateGameDto> {
  id: string;
  
}