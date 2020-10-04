using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace TikTakToe
{
    class TikTakToe
    {
        //[DllImport("kernel32.dll")]
        //static extern void OutputDebugString(string lpOutputString);
        private string cells = "_________";
        private readonly Dictionary<string, int> coordinates = new Dictionary<string, int>
        {
            {"1 3", 0 }, {"2 3", 1 }, {"3 3", 2 },
            {"1 2", 3 }, {"2 2", 4 }, {"3 2", 5 },
            {"1 1", 6 }, {"2 1", 7 }, {"3 1", 8 }
        };
        //private Dictionary<string, int> win_board = new Dictionary<string, int>
        //{
        //    {"user", 0 }, {"easy", 0 }, {"medium", 0 }, {"hard", 0 }, {"draw", 0}
        //};
        private enum Levels {
            User,
            Easy,
            Medium,
            Hard
        };
        public enum XO
        {
            X = -1,
            O = 1
        }
        private readonly Random rnd = new Random();
        private bool is_game_finished = false;
        private delegate void Game_Processes_Handler();
        private event Game_Processes_Handler TurnMadeEvent;
        //private bool test_condition = false;
        //private string command;
        public TikTakToe(string preset_cells)
        {
            cells = preset_cells;
            TurnMadeEvent += Print_Cells;
            TurnMadeEvent += Check_State_End;
        }

        public void Refresh_Game_State()
        {
            is_game_finished = false;
            cells = "_________";
            //if (win_board.Skip(1).Sum(x => x.Value) == 100)
            //{
            //    Console.WriteLine("Statistic for 100 games:\n" +
            //        "User\t -\t {0}\n" +
            //        "Easy\t -\t {1}\n" +
            //        "Medium\t -\t {2}\n" +
            //        "Hard\t -\t {3}\n" +
            //        "Draw\t -\t {4}", win_board["user"], win_board["easy"], win_board["medium"], win_board["hard"], win_board["draw"]);
            //    foreach (string key in new List<string>{ "user", "easy", "medium", "hard", "draw" })
            //    {
            //        win_board[key] = 0;
            //    }
            //}
        }
        public void Print_Cells()
        {
            //OutputDebugString(Get_Cells());
            Console.WriteLine(Get_Cells());
        }

        public string Get_Cells()
        {
            char[] char_cells = cells.ToCharArray();
            for (int i = 0; i < char_cells.Length; i++)
            {
                if (char_cells[i] == '_')
                {
                    char_cells[i] = ' ';
                }
            }
            char[] cells_in_a_row = new char[3];
            string returned_variant = "---------\n";
            for(int i = 0; i < 9; i += 3)
            {
                Array.Copy(char_cells, i, cells_in_a_row, 0, 3);
                returned_variant += string.Format("| {0} |\n", string.Join(" ", cells_in_a_row));
            }
            returned_variant += "---------\n";
            return returned_variant;
        }

        public void Game_Menu()
        {
            while(true)
            {
                Console.WriteLine("Please enter [start <player>] or [start <player1> <player2>] to start the game. " +
                    "\nPlayer options: " +
                    "\n\t1. user" +
                    "\n\t2. easy" +
                    "\n\t3. medium" +
                    "\n\t4. hard" +
                    "\nEnter 'exit' to stop the program");
                string command = Console.ReadLine();

                if (command == "exit")
                {
                    return;
                }
                else
                {
                    command = command.Substring(command.IndexOf(' '));
                    string[] players = command.Split(' ');
                    Print_Cells();
                    Start_Game(players);
                    Refresh_Game_State();
                }
                //else if (command == "test")
                //{
                //    test_condition = !test_condition;
                //    if (test_condition == false)
                //    {
                //        Refresh_Game_state();
                //    }
                //}
                //while (win_board.Skip(1).Sum(x => x.Value) != 100 && command != "exit" && command != "test")
                ////{
                //    command = command.Substring(command.IndexOf(' '));
                //    string[] players = command.Split(' ');
                //    Print_Cells();
                //    Start_Game(players);
                //    Refresh_Game_state();
                //    if (test_condition == false)
                //    {
                //        break;
                //    }
                //}
            }
        }

        public void Start_Game(string[] players)
        {
            while (!is_game_finished)
            {
                foreach (string player in players)
                {
                    if (!is_game_finished)
                    {
                        Play_Game(player);
                    }
                    else
                    {
                        return;
                    }
                }
            }
            //if (test_condition)
            //{
            //    string current_player = players[0];
            //    while (!is_game_finished)
            //    {
            //        foreach (string player in players)
            //        {
            //            if (!is_game_finished)
            //            {
            //                Play_Game(player);
            //                current_player = player;
            //            }
            //            else
            //            {
            //                if (Get_Game_State(cells) == "Draw")
            //                {
            //                    win_board["draw"] += 1;
            //                }
            //                else
            //                {
            //                    win_board[current_player]++;
            //                }
            //                return;
            //            }
            //        }
            //    }
            //    if (Get_Game_State(cells) == "Draw")
            //    {
            //        win_board["draw"] += 1;
            //    }
            //    else
            //    {
            //        win_board[current_player] += 1;
            //    }
            //    return;
            //}
        }

        public void Play_Game(string player)
        {
            if (player == "user")
            {
                Level_User();
            }
            else if (player == "easy")
            {
                Level_Easy();
            }
            else if (player == "medium")
            {
                Level_Medium();
            }
            else if (player == "hard")
            {
                Level_Hard();
            }
        }

        public void Level_User()
        {
            Console.WriteLine("Please, enter the coordinates:\n" +
                "---------------------\n" +
                "| {1 3} {2 3} {3 3} |\n" +
                "| {1 2} {2 2} {3 2} |\n" +
                "| {1 1} {2 1} {3 1} |\n" +
                "---------------------");
            string turn = Console.ReadLine();
            foreach (string coordinate in Get_Not_Occupied_Coordinates(cells))
            {
                if (coordinate == turn)
                {
                    cells = Make_Turn(cells, turn);
                    TurnMadeEvent();
                    return;
                }
            }
            Console.WriteLine("Error. Try again");
            Level_User();
        }

        public void Level_Easy()
        {
            List<string> not_occupied_cells = Get_Not_Occupied_Coordinates(cells);
            if (not_occupied_cells.Count > 0)
            {
                Console.WriteLine("Making move level easy");
                cells = Make_Turn(cells, not_occupied_cells[rnd.Next(not_occupied_cells.Count)]);
                TurnMadeEvent();
            }
        }

        public void Level_Medium() // Can be optimized
        {
            List<string> not_occupied_cells = Get_Not_Occupied_Coordinates(cells);
            if (not_occupied_cells.Count > 0)
            {
                Console.WriteLine("Making move level medium");
                foreach(string coordinate in not_occupied_cells)
                {
                    string ai_turn = coordinate;
                    string possible_move = Make_Turn(cells, ai_turn);
                    string future_state = Get_Game_State(possible_move);
                    if (future_state == "X wins" || future_state == "O wins")
                    {
                        cells = Make_Turn(cells, coordinate);
                        TurnMadeEvent();
                        return;
                    }
                    foreach(string coordinate2 in not_occupied_cells)
                    {
                        string future_state2 = Get_Game_State(Make_Turn(possible_move, coordinate2));
                        if (future_state2 == "X wins" || future_state2 == "O wins")
                        {
                            cells = Make_Turn(cells, coordinate2);
                            TurnMadeEvent();
                            return;
                        }
                    }
                }
                cells = Make_Turn(cells, not_occupied_cells[rnd.Next(not_occupied_cells.Count)]);
                TurnMadeEvent();
            }
        }

        public void Level_Hard()
        {
            List<string> not_occupied_cells = Get_Not_Occupied_Coordinates(cells);
            if (not_occupied_cells.Count > 0)
            {
                Console.WriteLine("Making move level hard");
                if (cells.Count(f => f == 'X') <= cells.Count(f => f == 'O'))
                {
                    cells = Make_Turn(cells, MiniMax(cells, XO.X, XO.X).Index);
                }
                else
                {
                    cells = Make_Turn(cells, MiniMax(cells, XO.O, XO.O).Index);
                }
                TurnMadeEvent();
            }
        }

        public MiniMaxNode MiniMax(string newboard, XO player, XO hard, double alpha=double.NegativeInfinity, double beta=double.PositiveInfinity)
        {
            List<string> not_occupied_cells = Get_Not_Occupied_Coordinates(newboard);
            string state = Get_Game_State(newboard);
            if (state.Contains(hard.ToString()))
            {
                return new MiniMaxNode { Score = 10 };
            }
            else if (hard == XO.X && state.Contains('O') || hard == XO.O && state.Contains('X'))
            {
                return new MiniMaxNode { Score = -10 };
            }
            else if (state == "Draw")
            {
                return new MiniMaxNode { Score = 0 };
            }
            //List<MiniMaxNode> moves = new List<MiniMaxNode>();
            //foreach(string coordinate in not_occupied_cells)
            //{
            //    moves.Add(new MiniMaxNode 
            //    { 
            //        Index = coordinates[coordinate], 
            //        Score = MiniMax(Make_Turn(newboard, coordinate), (XO)((int)player * -1), hard).Score 
            //    } 
            //    );
            //}
            MiniMaxNode best_move = null;
            if (player == hard)
            {
                double best_score = double.NegativeInfinity;
                foreach(string coordinate in not_occupied_cells)
                {
                    double value = MiniMax(Make_Turn(newboard, coordinate), (XO)((int)player * -1), hard, alpha, beta).Score;
                    if (value > best_score)
                    {
                        best_score = value;
                        best_move = new MiniMaxNode { Index = coordinates[coordinate], Score = best_score };
                        alpha = Math.Max(alpha, best_score);
                    }
                    if (beta <= alpha)
                    {
                        break;
                    }
                }
                return best_move;
            }
            else
            {
                double best_score = double.PositiveInfinity;
                foreach (string coodrinate in not_occupied_cells)
                {
                    double value = MiniMax(Make_Turn(newboard, coodrinate), (XO)((int)player * -1), hard, alpha, beta).Score;
                    if (value < best_score)
                    {
                        best_score = value;
                        best_move = new MiniMaxNode { Index = coordinates[coodrinate], Score = best_score };
                        beta = Math.Max(alpha, best_score);
                    }
                    if (beta <= alpha)
                    {
                        break;
                    }
                }
                return best_move;
            }
        }

        public string Make_Turn(string cells, object turn)
        {
            int x = cells.Count(f => f == 'X');
            int o = cells.Count(f => f == 'O');
            List<char> cells_list = cells.ToList();
            if (turn is string)
            {
                if (x <= o)
                {
                    cells_list[coordinates[(string)turn]] = 'X';
                }
                else
                {
                    cells_list[coordinates[(string)turn]] = 'O';
                }
            }
            if (turn is int)
            {
                if (x <= o)
                {
                    cells_list[(int)turn] = 'X';
                }
                else
                {
                    cells_list[(int)turn] = 'O';
                }
            }
            return string.Join("", cells_list);
        }

        public void Check_State_End()
        {
            string state = Get_Game_State(cells);
            if (state != "Game is not finished")
            {
                Console.WriteLine(state);
                is_game_finished = true;
            }
            else
            {
                is_game_finished = false;
            }
        }

        public string Get_Game_State(string cells)
        {
            char[] char_cells = cells.ToCharArray();
            char[] line = new char[3];
            char[] column = new char[3];
            char[] leading_diagonal = new char[3];
            char[] even_diagonal = new char[3];
            for (int i = 0, n = 0, m = 2, g = 0; i < 9; i += 3, n += 4, m += 2, g++)
            {
                for (int j = i, l = 0, c = g; j < (i+3); j++, l++, c += 3)
                {
                    line[l] = char_cells[j];
                    column[l] = char_cells[c];
                }
                if(line.Count(f => f == 'O') == 3 || column.Count(f => f == 'O') == 3)
                {
                    return "O wins";
                }
                if(line.Count(f => f == 'X') == 3 || column.Count(f => f == 'X') == 3)
                {
                    return "X wins";
                }
                leading_diagonal[g] = char_cells[n];
                even_diagonal[g] = char_cells[m];
            }
            if(leading_diagonal.Count(f => f == 'O') == 3 || even_diagonal.Count(f => f == 'O') == 3)
            {
                return "O wins";
            }
            if(leading_diagonal.Count(f => f == 'X') == 3 || even_diagonal.Count(f => f == 'X') == 3)
            {
                return "X wins";
            }
            if(cells.Count(f => f == 'X') + cells.Count(f => f == 'O') == 9)
            {
                return "Draw";
            }
            return "Game is not finished";
        }

        public List<string> Get_Not_Occupied_Coordinates(string cells)
        {
            List<string> not_occupied = new List<string>();
            foreach (KeyValuePair<string, int> entry in coordinates)
            {
                if (cells[entry.Value] == '_')
                {
                    not_occupied.Add(entry.Key);
                }
            }
            return not_occupied;
        }
    }
}
