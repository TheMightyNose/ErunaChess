﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErunaChess
{
	static class MakeMove
	{
		public static void ClearPiece(Board board, int square)
		{
			int piece = board.board[square];

			board.board[square] = Global.empty;

			for(int i = 0; i < board.pieces[piece].Count; i++)
			{
				if(board.pieces[piece][i] == square)
				{
					board.pieces[piece].RemoveAt(i);
					break;
				}
			}
		}
		public static void AddPiece(Board board, int square, int piece)
		{
			board.board[square] = piece;

			board.pieces[piece].Add(square);
		}

		public static void MovePiece(Board board, int from, int to)
		{
			int piece = board.board[from];

			board.board[from] = Global.empty;

			board.board[to] = Global.empty;

			for (int i = 0; i < board.pieces[piece].Count; i++)
			{
				if (board.pieces[piece][i] == from)
				{
					board.pieces[piece][i] = to;
					break;
				}
			}
		}

		public static void Make(Board board, int move)
		{
			int from = Move.From(move);
			int to = Move.From(move);

			if((move & Move.EnPassantFlag()) > 0)
				ClearPiece(board, board.side == Global.white ? to - Global.boardWidth : to + Global.boardWidth);

			int captured = Move.Captured(move);
			board.fiftyMove++;

			if(captured != Global.empty)
			{
				ClearPiece(board, to);
				board.fiftyMove = 0;
			}

			if ((move & Move.CastleFlag()) > 0)
			{
				//castle
			}

			board.history[board.historyPly].move = move;
			board.history[board.historyPly].fiftymove = board.fiftyMove;
			board.history[board.historyPly].enpassantSquare = board.enpassantSquare;
			board.history[board.historyPly].move = move;

			//update castle permissions

			board.enpassantSquare = (int)Global.Square.offBoard;

			board.ply++;
			board.historyPly++;

			//check if pawn move

			MovePiece(board, from, to);

			int promoted = Move.Promoted(move);

			if (promoted != Global.empty)
			{
				ClearPiece(board, to);
				AddPiece(board, to, promoted);
			}

			board.side ^= Global.border;

			if (Attack.SquareAttacked(board, board.pieces[Global.kingBits + (board.side & Global.border)][0], board.side))
			{
				//unmake
			}
		}
	}
}
