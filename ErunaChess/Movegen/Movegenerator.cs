﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErunaChess
{
	public static class MoveGenerator
	{
		static readonly int[] knightDirections = { 33, 31, -33, -31, 18, 14, -18, -14 };
		static readonly int[] kingDirections = { 1, -1, 16, 15, 17, -16, -15, -17 };
		static readonly int[] rookDirections = { 1, -1, 16, -16 };
		static readonly int[] bishopDirections = { 15, 17, -15, -17 };

		public static void GenerateAllMoves(Board board, MoveList moveList)
		{
			int enemy = board.side ^ Global.border;
			
			// generate white pawn moves
			if (board.side == Global.white)
			{
				for (int i = 0; i < board.pieceCount[(int)Board.Pieces.whitePawn]; i++)
				{
					int square = board.pieces[(int)Board.Pieces.whitePawn, i];
					// if 7th rank, promotion fun.
					if (square >= (int)Global.Square.A7)
					{
						if (board.board[square + Global.boardWidth] == Global.empty)	
							AddMove.PromotionMove(board, Move.Write(square, square + Global.boardWidth, 0, 0, false, false, false), moveList);
						//captures
						if ((board.board[square + Global.boardWidth + 1] & Global.border) == enemy)
							AddMove.PromotionMove(board, Move.Write(square, square + Global.boardWidth + 1, board.board[square + Global.boardWidth + 1], 0, false, false, false), moveList);

						if ((board.board[square + Global.boardWidth - 1] & Global.border) == enemy)
							AddMove.PromotionMove(board, Move.Write(square, square + Global.boardWidth - 1, board.board[square + Global.boardWidth - 1], 0, false, false, false), moveList);
					}
					else
					{
						if (board.board[square + Global.boardWidth] == Global.empty)
						{
							AddMove.QuietPawnMove(board, Move.Write(square, square + Global.boardWidth, 0, 0, false, false, false), moveList);

							if (square <= (int)Global.Square.H2 && board.board[square + Global.boardWidth * 2] == Global.empty)
								AddMove.QuietPawnMove(board, Move.Write(square, square + Global.boardWidth * 2, 0, 0, false, true, false), moveList);
						}
						//captures
						if ((board.board[square + Global.boardWidth + 1] & Global.border) == enemy)
							AddMove.PawnCaptureMove(board, Move.Write(square, square + Global.boardWidth + 1, board.board[square + Global.boardWidth + 1], 0, false, false, false), moveList);

						if ((board.board[square + Global.boardWidth - 1] & Global.border) == enemy)
							AddMove.PawnCaptureMove(board, Move.Write(square, square + Global.boardWidth - 1, board.board[square + Global.boardWidth - 1], 0, false, false, false), moveList);
						//enpassant
						if (square + Global.boardWidth + 1 == board.enpassantSquare)
							AddMove.EnpassantMove(board, Move.Write(square, board.enpassantSquare, board.board[board.enpassantSquare], 0, true, false, false), moveList);

						if (square + Global.boardWidth - 1 == board.enpassantSquare)
							AddMove.EnpassantMove(board, Move.Write(square, board.enpassantSquare, board.board[board.enpassantSquare], 0, true, false, false), moveList);
					}
				}
				//castling
				if ((board.castlePermission & Global.whiteKingSideCastle) > 0)
				{
					if (board.board[73] == Global.empty && board.board[74] == Global.empty)
					{
						if (Attack.SquareAttacked(enemy, 73, board) && Attack.SquareAttacked(enemy, 74, board))
							AddMove.QuietMove(board, Move.Write(72, 74, 0, 0, false, false, true), moveList);
					}
				}

				if ((board.castlePermission & Global.whiteQueenSideCastle) > 0)
				{
					if (board.board[71] == Global.empty && board.board[70] == Global.empty)
					{
						if (Attack.SquareAttacked(enemy, 71, board) && Attack.SquareAttacked(enemy, 70, board))
							AddMove.QuietMove(board, Move.Write(72, 70, 0, 0, false, false, true), moveList);
					}
				}
			}
			// generate black pawn moves
			else
			{
				for (int i = 0; i < board.pieceCount[(int)Board.Pieces.blackPawn]; i++)
				{
					int square = board.pieces[(int)Board.Pieces.blackPawn, i];
					// if 2nd rank, promotion fun.
					if ( square <= (int)Global.Square.H2)
					{
						if (board.board[square - Global.boardWidth] == Global.empty)
							AddMove.PromotionMove(board, Move.Write(square, square - Global.boardWidth, 0, 0, false, false, false), moveList);
						//captures
						if ((board.board[square - Global.boardWidth + 1] & Global.border) == enemy)
							AddMove.PromotionMove(board, Move.Write(square, square - Global.boardWidth + 1, board.board[square - Global.boardWidth + 1], 0, false, false, false), moveList);

						if ((board.board[square - Global.boardWidth - 1] & Global.border) == enemy)
							AddMove.PromotionMove(board, Move.Write(square, square - Global.boardWidth - 1, board.board[square - Global.boardWidth - 1], 0, false, false, false), moveList);
					}
					else
					{
						if (board.board[square - Global.boardWidth] == Global.empty)
						{
							AddMove.QuietPawnMove(board, Move.Write(square, square - Global.boardWidth, 0, 0, false, false, false), moveList);

							if (square >= (int)Global.Square.A7 && board.board[square - Global.boardWidth * 2] == Global.empty)
								AddMove.QuietPawnMove(board, Move.Write(square, square - Global.boardWidth * 2, 0, 0, false, true, false), moveList);
						}
						//captures
						if ((board.board[square - Global.boardWidth + 1] & Global.border) == enemy)
							AddMove.PawnCaptureMove(board, Move.Write(square, square - Global.boardWidth + 1, board.board[square - Global.boardWidth + 1], 0, false, false, false), moveList);

						if ((board.board[square - Global.boardWidth - 1] & Global.border) == enemy)
							AddMove.PawnCaptureMove(board, Move.Write(square, square - Global.boardWidth - 1, board.board[square - Global.boardWidth - 1], 0, false, false, false), moveList);
						//enpassant
						if (square - Global.boardWidth + 1 == board.enpassantSquare)
							AddMove.EnpassantMove(board, Move.Write(square, board.enpassantSquare, board.board[board.enpassantSquare], 0, true, false, false), moveList);

						if (square - Global.boardWidth - 1 == board.enpassantSquare)
							AddMove.EnpassantMove(board, Move.Write(square, board.enpassantSquare, board.board[board.enpassantSquare], 0, true, false, false), moveList);
					}
				}
				//castling
				if ((board.castlePermission & Global.blackKingSideCastle) > 0)
				{
					if (board.board[185] == Global.empty && board.board[186] == Global.empty)
					{
						if (Attack.SquareAttacked(enemy, 185, board) && Attack.SquareAttacked(enemy, 186, board))
							AddMove.QuietMove(board, Move.Write(184, 186, 0, 0, false, false, true), moveList);
					}
				}

				if ((board.castlePermission & Global.blackQueenSideCastle) > 0)
				{
					if (board.board[183] == Global.empty && board.board[182] == Global.empty)
					{
						if (Attack.SquareAttacked(enemy, 183, board) && Attack.SquareAttacked(enemy, 182, board))
							AddMove.QuietMove(board, Move.Write(184, 182, 0, 0, false, false, true), moveList);
					}
				}
			} 

			//adding methods for slider move and non slider generation would save quite some lines.
			int knight = board.side == Global.white ? (int)Board.Pieces.whiteKnight : (int)Board.Pieces.blackKnight;
			for (int i = 0; i < board.pieceCount[knight]; i++)
			{
				int square = board.pieces[knight, i];
				for (int j = 0; j < 8; j++)
				{
					int toSquare = square + knightDirections[j];
					if (board.board[toSquare] == Global.empty)
					{
						AddMove.QuietMove(board, Move.Write(square, toSquare, 0, 0, false, false, false), moveList);
					}
					if ((board.board[toSquare] & Global.border) == enemy)
					{
						AddMove.CaptureMove(board, Move.Write(square, toSquare, board.board[toSquare], 0, false, false, false), moveList);
					}
				}
			}

			int king = board.side == Global.white ? (int)Board.Pieces.whiteKing : (int)Board.Pieces.blackKing;
			for (int i = 0; i < board.pieceCount[king]; i++) // a bit odd, there is always only one king
			{
				int square = board.pieces[king, i];
				for (int j = 0; j < 8; j++)
				{
					int toSquare = square + kingDirections[j];
					if (board.board[toSquare] == Global.empty)
					{
						AddMove.QuietMove(board, Move.Write(square, toSquare, 0, 0, false, false, false), moveList);
					}
					if ((board.board[toSquare] & Global.border) == enemy)
					{
						AddMove.CaptureMove(board, Move.Write(square, toSquare, board.board[toSquare], 0, false, false, false), moveList);
					}
				}
			}

			int bishop = board.side == Global.white ? (int)Board.Pieces.whiteBishop : (int)Board.Pieces.blackBishop;
			for (int i = 0; i < board.pieceCount[bishop]; i++)
			{
				int square = board.pieces[bishop, i];
				for (int j = 0; j < 4; j++)
				{
					int toSquare = square + bishopDirections[j];
					while(true)
					{
						if ((board.board[toSquare] & Global.border) > 0)
						{
							if ((board.board[toSquare] & Global.border) == enemy)
							{
								AddMove.CaptureMove(board, Move.Write(square, toSquare, board.board[toSquare], 0, false, false, false), moveList);
							}
							break;
						}
						AddMove.QuietMove(board, Move.Write(square, toSquare, 0, 0, false, false, false), moveList);
						toSquare += bishopDirections[j];
					}
				}
			}

			int rook = board.side == Global.white ? (int)Board.Pieces.whiteRook : (int)Board.Pieces.blackRook;
			for (int i = 0; i < board.pieceCount[rook]; i++)
			{
				int square = board.pieces[rook, i];
				for (int j = 0; j < 4; j++)
				{
					int toSquare = square + rookDirections[j];
					while (true)
					{
						if ((board.board[toSquare] & Global.border) > 0)
						{
							if ((board.board[toSquare] & Global.border) == enemy)
							{
								AddMove.CaptureMove(board, Move.Write(square, toSquare, board.board[toSquare], 0, false, false, false), moveList);
							}
							break;
						}
						AddMove.QuietMove(board, Move.Write(square, toSquare, 0, 0, false, false, false), moveList);
						toSquare += rookDirections[j];
					}
				}
			}

			int queen = board.side == Global.white ? (int)Board.Pieces.whiteQueen : (int)Board.Pieces.blackQueen;
			for (int i = 0; i < board.pieceCount[queen]; i++)
			{
				int square = board.pieces[queen, i];
				for (int j = 0; j < 4; j++)
				{
					int toSquare = square + kingDirections[j];
					while (true)
					{
						if ((board.board[toSquare] & Global.border) > 0)
						{
							if ((board.board[toSquare] & Global.border) == enemy)
							{
								AddMove.CaptureMove(board, Move.Write(square, toSquare, board.board[toSquare], 0, false, false, false), moveList);
							}
							break;
						}
						AddMove.QuietMove(board, Move.Write(square, toSquare, 0, 0, false, false, false), moveList);
						toSquare += kingDirections[j];
					}
				}
			}

			Console.WriteLine(moveList.count + " moves");

			for (int i = 0; i < moveList.count; i++ )
			{
				Console.WriteLine();
				Console.WriteLine(Move.From(moveList.moves[i].move) + " from");
				Console.WriteLine(Move.To(moveList.moves[i].move) + " to");
			}
		}
	}
}
