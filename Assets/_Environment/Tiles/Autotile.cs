using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Randolph.Core {
	public class Autotile : Tile {
		[Flags]
		enum Ne {
			TopRight = 1, Top = 2, TopLeft = 4, Left = 8, DownLeft = 16, Down = 32, DownRight = 64, Right = 128,
			TopHalf = TopRight | Top | TopLeft, DownHalf = DownLeft | Down | DownRight,
			LeftHalf = TopLeft | Left | DownLeft, RightHalf = TopRight | Right | DownRight,
			All = TopHalf | DownHalf | Left | Right,
			Sides = Top | Down | Left | Right,
			Corners = TopRight | TopLeft | DownRight | DownLeft
		}

		public Sprite[] sprites = new Sprite[Constants.Tilemap.tileCount];

		public override void RefreshTile(Vector3Int position, ITilemap tilemap) {
			for (int j = -1; j <= 1; ++j)
				for (int i = -1; i <= 1; ++i) {
					var neighborPos = new Vector3Int(position.x + i, position.y + j, position.z);
					if (IsAutotile(tilemap, neighborPos))
						tilemap.RefreshTile(neighborPos);
				}
		}

		public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData) {
			Ne neighbors = 0;
			if (IsAutotile(tilemap, new Vector3Int(position.x + 1, position.y + 1, position.z)))
				neighbors |= Ne.TopRight;
			if (IsAutotile(tilemap, new Vector3Int(position.x, position.y + 1, position.z)))
				neighbors |= Ne.Top;
			if (IsAutotile(tilemap, new Vector3Int(position.x - 1, position.y + 1, position.z)))
				neighbors |= Ne.TopLeft;
			if (IsAutotile(tilemap, new Vector3Int(position.x - 1, position.y, position.z)))
				neighbors |= Ne.Left;
			if (IsAutotile(tilemap, new Vector3Int(position.x - 1, position.y - 1, position.z)))
				neighbors |= Ne.DownLeft;
			if (IsAutotile(tilemap, new Vector3Int(position.x, position.y - 1, position.z)))
				neighbors |= Ne.Down;
			if (IsAutotile(tilemap, new Vector3Int(position.x + 1, position.y - 1, position.z)))
				neighbors |= Ne.DownRight;
			if (IsAutotile(tilemap, new Vector3Int(position.x + 1, position.y, position.z)))
				neighbors |= Ne.Right;

			tileData.sprite = ByNeighbors(neighbors);
		}

		private Sprite ByNeighbors(Ne neighbors) {
			if (Matches(neighbors, Ne.All))
				return sprites[0];
			else if (Matches(neighbors, Ne.All & ~Ne.TopLeft))
				return sprites[1];
			else if (Matches(neighbors, Ne.All & ~Ne.TopRight))
				return sprites[2];
			else if (Matches(neighbors, Ne.All & ~Ne.TopLeft & ~Ne.TopRight))
				return sprites[3];
			else if (Matches(neighbors, Ne.All & ~Ne.DownRight))
				return sprites[4];
			else if (Matches(neighbors, Ne.All & ~Ne.TopLeft & ~Ne.DownRight))
				return sprites[5];
			else if (Matches(neighbors, Ne.All & ~Ne.TopRight & ~Ne.DownRight))
				return sprites[6];
			else if (Matches(neighbors, Ne.All & ~Ne.Corners | Ne.DownLeft))
				return sprites[7];
			else if (Matches(neighbors, Ne.All & ~Ne.DownLeft))
				return sprites[8];
			else if (Matches(neighbors, Ne.All & ~Ne.TopLeft & ~Ne.DownLeft))
				return sprites[9];
			else if (Matches(neighbors, Ne.All & ~Ne.TopRight & ~Ne.DownLeft))
				return sprites[10];
			else if (Matches(neighbors, Ne.All & ~Ne.Corners | Ne.DownRight))
				return sprites[11];
			else if (Matches(neighbors, Ne.All & ~Ne.DownLeft & ~Ne.DownRight))
				return sprites[12];
			else if (Matches(neighbors, Ne.All & ~Ne.Corners | Ne.TopRight))
				return sprites[13];
			else if (Matches(neighbors, Ne.All & ~Ne.Corners | Ne.TopLeft))
				return sprites[14];
			else if (Matches(neighbors, Ne.All & ~Ne.Corners))
				return sprites[15];
			else if (Matches(neighbors, Ne.All & ~Ne.LeftHalf))
				return sprites[16];
			else if (Matches(neighbors, Ne.All & ~Ne.LeftHalf & ~Ne.TopRight))
				return sprites[17];
			else if (Matches(neighbors, Ne.All & ~Ne.LeftHalf & ~Ne.DownRight))
				return sprites[18];
			else if (Matches(neighbors, Ne.All & ~Ne.LeftHalf & ~Ne.TopRight & ~Ne.DownRight))
				return sprites[19];
			else if (Matches(neighbors, Ne.All & ~Ne.TopHalf))
				return sprites[20];
			else if (Matches(neighbors, Ne.All & ~Ne.TopHalf & ~Ne.DownRight))
				return sprites[21];
			else if (Matches(neighbors, Ne.All & ~Ne.TopHalf & ~Ne.DownLeft))
				return sprites[22];
			else if (Matches(neighbors, Ne.All & ~Ne.TopHalf & ~Ne.DownRight & ~Ne.DownLeft))
				return sprites[23];
			else if (Matches(neighbors, Ne.All & ~Ne.RightHalf))
				return sprites[24];
			else if (Matches(neighbors, Ne.All & ~Ne.RightHalf & ~Ne.DownLeft))
				return sprites[25];
			else if (Matches(neighbors, Ne.All & ~Ne.RightHalf & ~Ne.TopLeft))
				return sprites[26];
			else if (Matches(neighbors, Ne.All & ~Ne.RightHalf & ~Ne.TopLeft & ~Ne.DownLeft))
				return sprites[27];
			else if (Matches(neighbors, Ne.All & ~Ne.DownHalf))
				return sprites[28];
			else if (Matches(neighbors, Ne.All & ~Ne.DownHalf & ~Ne.TopLeft))
				return sprites[29];
			else if (Matches(neighbors, Ne.All & ~Ne.DownHalf & ~Ne.TopRight))
				return sprites[30];
			else if (Matches(neighbors, Ne.All & ~Ne.DownHalf & ~Ne.TopLeft & ~Ne.TopRight))
				return sprites[31];
			else if (Matches(neighbors, Ne.Top | Ne.Down))
				return sprites[32];
			else if (Matches(neighbors, Ne.Left | Ne.Right))
				return sprites[33];
			else if (Matches(neighbors, Ne.Down | Ne.Right | Ne.DownRight))
				return sprites[34];
			else if (Matches(neighbors, Ne.Down | Ne.Right))
				return sprites[35];
			else if (Matches(neighbors, Ne.Down | Ne.Left | Ne.DownLeft))
				return sprites[36];
			else if (Matches(neighbors, Ne.Down | Ne.Left))
				return sprites[37];
			else if (Matches(neighbors, Ne.Top | Ne.Left | Ne.TopLeft))
				return sprites[38];
			else if (Matches(neighbors, Ne.Top | Ne.Left))
				return sprites[39];
			else if (Matches(neighbors, Ne.Top | Ne.Right | Ne.TopRight))
				return sprites[40];
			else if (Matches(neighbors, Ne.Top | Ne.Right))
				return sprites[41];
			else if (Matches(neighbors, Ne.Down))
				return sprites[42];
			else if (Matches(neighbors, Ne.Right))
				return sprites[43];
			else if (Matches(neighbors, Ne.Top))
				return sprites[44];
			else if (Matches(neighbors, Ne.Left))
				return sprites[45];
			else
				return sprites[46];
		}

		private bool IsAutotile(ITilemap tilemap, Vector3Int pos) {
			return tilemap.GetTile(pos) == this;
		}

		private bool Matches(Ne lhs, Ne rhs) {
			return (lhs & rhs) == rhs;
		}
	}
}