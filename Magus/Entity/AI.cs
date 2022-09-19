using Magus.GameBoard;
using Magus.Items;
using Magus.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Magus.Entity
{
    public class AI
    {
        private GameManager manager;
        private List<Location> pathTo = new List<Location>();
        private GridMoveTile[,] moveArea;

        public AI(GameManager manager)
        {
            this.manager = manager;
            moveArea = new GridMoveTile[Constants.cAreaSize, Constants.cAreaSize];
            InitilizeArea();
        }


        //Creates all tiles in the movearea
        private void InitilizeArea()
        {
            for(int x = 0; x < Constants.cAreaSize; x++)
            {
                for(int y = 0; y < Constants.cAreaSize; y++)
                {
                    moveArea[x, y] = new GridMoveTile();
                }
            }
        }

        // Determines the Direction of where to go
        private int DetermineDirection(int val)
        {
            if (val > 0)
                return 1;
            if (val < 0)
                return -1;
            return 0;

        }

        // Moves Character towards a given x and y value;
        // Causes Game Stall When walking into wall
        public bool GoTowards(Character c, int x, int y)
        {
            int dx = DetermineDirection(x - c.Location.X);
            int dy = DetermineDirection(y - c.Location.Y);

            if (FindPath(c, x, y, dx, dy) || FindDirection(c, dx, dy))
            {
                if (Move(c, new Location(c.Location.X + dx, c.Location.Y + dy)))
                    return true;
                return false;
            }
            return false;
        }

        // Finds a valid path through the world
        private bool FindPath(Character c, int tx, int ty, int dx, int dy)
        {
            int x, y, min = 0, cost;
            if (Math.Max(Math.Abs(tx - c.Location.X), Math.Abs(ty - c.Location.Y)) > Constants.cMaxStep)
                return false;

            x = (c.Location.X + tx) / 2;
            y = (c.Location.Y + ty) / 2;
            x -= Constants.cAreaSize / 2;
            y -= Constants.cAreaSize / 2;
            InitArea(c, x, y);
            WalkPath(tx - x, ty - y, Constants.cMaxStep, 0);

            x = c.Location.X - x;
            y = c.Location.Y - y;
            cost = moveArea[x + 1, y].Path;

            if (cost > 0 && (min == 0 || cost < min))
            {
                min = cost;
                dx = 1; dy = 0;
            }
            cost = moveArea[x + 1, y + 1].Path;
            if (cost > 0 && (min == 0 || cost < min))
            {
                min = cost;
                dx = 1; dy = 1;
            }
            cost = moveArea[x, y + 1].Path;
            if (cost > 0 && (min == 0 || cost < min))
            {
                min = cost;
                dx = 0; dy = 1;
            }
            cost = moveArea[x - 1, y + 1].Path;
            if (cost > 0 && (min == 0 || cost < min))
            {
                min = cost;
                dx = -1; dy = 1;
            }
            cost = moveArea[x - 1, y].Path;
            if (cost > 0 && (min == 0 || cost < min))
            {
                min = cost;
                dx = -1; dy = 0;
            }
            cost = moveArea[x - 1, y - 1].Path;
            if (cost > 0 && (min == 0 || cost < min))
            {
                min = cost;
                dx = -1; dy = -1;
            }
            cost = moveArea[x, y - 1].Path;
            if (cost > 0 && (min == 0 || cost < min))
            {
                min = cost;
                dx = 0; dy = -1;
            }
            cost = moveArea[x + 1, y - 1].Path;
            if (cost > 0 && (min == 0 || cost < min))
            {
                min = cost;
                dx = 1; dy = -1;
            }
            return (min != 0);
        }

        //Find the direction of where to move
        private bool FindDirection(Character c, int dx, int dy) 
        {
            int cx = c.Location.X;
            int cy = c.Location.Y;
            if(!manager.IsTileBlocked(cx + dx, cy + dy))
                return true;
            else
                if (dx == 0)
            {
                if (!manager.IsTileBlocked(cx - 1, cy + dy))
                    dx = -1;
                else if (!manager.IsTileBlocked(cx + 1, cy + dy))
                    dx = 1;
                else if (!manager.IsTileBlocked(cx - 1, cy))
                { dx = -1; dy = 0; }
                else if (!manager.IsTileBlocked(cx + 1, cy))
                { dx = 1; dy = 0; }
                else return false;
            }
            else
            if (dy == 0)
            {
                if (!manager.IsTileBlocked(cx + dx, cy - 1))
                    dy = -1;
                else if (!manager.IsTileBlocked(cx + dx, cy + 1))
                    dy = 1;
                else if (!manager.IsTileBlocked(cx, cy - 1))
                { dx = 0; dy = -1; }
                else if (!manager.IsTileBlocked(cx, cy + 1))
                { dx = 0; dy = 1; }
                else return false;
            }
            else
            {
                if (!manager.IsTileBlocked(cx, cy + dy))
                    dx = 0;
                if (!manager.IsTileBlocked(cx + dx, cy))
                    dy = 0;
                if (!manager.IsTileBlocked(cx - dx, cy + dy))
                    dx = -dx;
                if (!manager.IsTileBlocked(cx + dx, cy - dy))
                    dy = -dy;
                else
                    return false;
            }
            return true;
        }

        //Initilizes the Movement array
        private void InitArea(Character c, int cx, int cy)
        {

            for (int x = 0; x < Constants.cAreaSize; x++)
                for (int y = 0; y < Constants.cAreaSize; y++)
                {
                    if (!manager.IsTileBlocked(cx + x, cy + y))
                    {
                        moveArea[x, y].Step = 1;
                        moveArea[x, y].Path = 10000;
                    }
                }
        }

        //Checks a Paths cost
        private void WalkPath(int x, int y, int step, int cost)
        {
            GridMoveTile tile;

            if (step > 0 && 
                x >= 0 && x < Constants.cAreaSize && 
                y >= 0 && y < Constants.cAreaSize)
            {
                tile = moveArea[x, y];
                if (step == Constants.cMaxStep || (tile.Step > 0 && tile.Path > tile.Step + cost))
                {
                    cost += tile.Step;
                    tile.Path = cost;
                    step--;
                    WalkPath(x + 1, y, step, cost);
                    WalkPath(x + 1, y + 1, step, cost);
                    WalkPath(x, y + 1, step, cost);
                    WalkPath(x - 1, y + 1, step, cost);
                    WalkPath(x - 1, y, step, cost);
                    WalkPath(x - 1, y - 1, step, cost);
                    WalkPath(x, y - 1, step, cost);
                    WalkPath(x + 1, y - 1, step, cost);
                }
            }
        }
        
        //Sets a players Location to a given location
        /*private void MovePlayerToLocation(Location location)
        {
            Move(cManager.GetPlayer(), location);
        }*/


        //Sets a characters Location to a given location
        private bool Move(Character c, Location location) // remove a movement point from the character
        {
            if(!manager.IsTileBlocked(location.X, location.Y))
            {
                c.Location = location;
                c.Moves--;
                manager.SetCamera();
                return true;
            }
            return false;
        }

        public void AttackPlayer(Character attacker, Item equipeditem)
        {
            if (attacker.MP > 0)
            {
                //spell attack
            }
            else
            {
                //commit a felony
            }
        }

        //Logic for the AI basic turn
        public void RunAITurn(Character pc)
        {
            int cameraBoundX = pc.Location.X - (Constants.CAMERA_VIEW_SIZE / 2);
            int cameraBoundY = pc.Location.Y - (Constants.CAMERA_VIEW_SIZE / 2);
            Grid gameBoard = manager.GetGameBoard();
            for (int x = 0; x < Constants.CAMERA_VIEW_SIZE; x++)
                for (int y = 0; y < Constants.CAMERA_VIEW_SIZE; y++)
                {
                    if (manager.GetCharacterManager().LocationContainsCharacter(new Location(cameraBoundX + x, cameraBoundY + y)))
                    {
                        Character c = manager.GetCharacterManager().FindCharacter(cameraBoundX + x, cameraBoundY + y);
                        if (c != null)
                        {
                            if (c.Opponent == null)
                                c.Opponent = pc;
                            //Movement
                            while(c.Moves > 0)
                            {
                                if (GoTowards(c, c.Opponent.Location.X, c.Opponent.Location.Y))
                                    manager.SetCamera();
                                else
                                {
                                    Combat(c);
                                    c.Moves--;
                                }
                                    
                                if (manager.GetCharacterManager().Player.HP <= 0)
                                    break;
                            }
                        }
                    }
                }
            manager.ChangeTurnState(TurnState.player);
        }

        public void Combat(Character c)
        {
            if (c.Opponent == null)
                return;
            if(ValidTarget(c.Opponent, c.Location.X - c.Opponent.Location.X, c.Location.Y - c.Opponent.Location.Y, c.Opponent.IsNasty) && 
                ValidTarget(c, c.Opponent.Location.X - c.Location.X, c.Opponent.Location.Y - c.Location.Y, c.IsNasty))
            {
                Action(c, c.Opponent.Location.X - c.Location.X, c.Opponent.Location.Y - c.Location.Y);
            }
            return;
        }

        //Checks if a player is a valid target
        private bool ValidTarget(Character target, int dx, int dy, bool isNasty)
        {
            int x = 0;
            int y = 0;
            if (Math.Abs(dx) > Constants.CAMERA_VIEW_SIZE / 2 || Math.Abs(dy) > Constants.CAMERA_VIEW_SIZE / 2)
                return false;
            x = target.Location.X + dx;
            y = target.Location.Y + dy;
            x += DetermineDirection(target.Location.X - x);
            y += DetermineDirection(target.Location.Y - y);

            while(x != target.Location.X || y != target.Location.Y)
            {
                if (manager.GetCharacterManager().LocationContainsCharacter(new Location(x, y)))
                    if (manager.GetCharacterManager().FindCharacter(x, y).IsNasty && isNasty)
                        return false;
                x += DetermineDirection(target.Location.X - x);
                y += DetermineDirection(target.Location.Y - y);
            }
            return true;
        }

        //conatins a list of actions which can be used depending on item used
        public void Action(Character c, int dx, int dy)
        {
            //Add More Attacking Types
            Strike(c, null, dx, dy);
        }

        //Strike comabt action
        public void Strike(Character c, Item item, int dx, int dy)
        {
            if (c.Moves == 0)
                return;

            dx = DetermineDirection(dx);
            dy = DetermineDirection(dy);

            //Play Swing Sound


            if (!ValidTarget(c, dx, dy, c.IsNasty))
                return;

            if (manager.GetCharacterManager().LocationContainsCharacter(new Location(c.Location.X + dx, c.Location.Y + dy)))
            {
                do
                {
                    bool success = SkillRoll(c, item);
                    if (success)
                    {
                        int damage = GetDamage(c, item);
                        damage++;
                        damage += c.Strength >> 2;
                        Violence(c, dx, dy, damage);
                    }
                    // if (item == null)
                    // item = item->next;
                }
                while (item != null);
            }
        }

        public bool SkillRoll(Character c, Item item)
        {
            Random random = new Random();
            int skill, die;
            skill = c.Skill;
            //if (item != null)
            //skill += item.Modifier;

            skill = Math.Min(skill, 19);
            die = random.Next(20) + 1;

            if (die <= skill)
            {
                if (die <= 3)
                {
                    Experience(c, 2);
                    return true;
                }
                Experience(c, 1);
                return true;
            }
            return false;
        }

        //Gets damage when item and damage modifier are added
        public int GetDamage(Character c, Item item)
        {
            int damage;
            if (item == null)
                return 0;
            damage = item.Value;
            //damage += item.Modifier;
            //damage += c.DamageBonus; If you use berserk spell

            return damage;
        }

        public void Experience(Character c, int xp)
        {
            c.XP += xp;

            //levelup code goes here

        }

        //Basic damage method which is used for most damage types
        public void Violence(Character c, int dx, int dy, int damage)
        {
            int x, y;
            x = c.Location.X + dx;
            y = c.Location.Y + dy;
            Character target = manager.GetCharacterManager().FindCharacter(x, y);
            if (target == null)
            {
                Character player = manager.GetCharacterManager().Player;
                if (player.Location.X == x && player.Location.Y == y)
                {
                    target = player;
                }
            }
            Image icon = target.Icon;
            target.Icon = Properties.Resources.EffectBam;
            manager.SetCamera();
            Damage(x, y, damage, target);
            Thread.Sleep(15);
            target.Icon = icon;
            manager.SetCamera();
        }

        //Applies damage to character
        public void Damage(int x, int y, int damage, Character c)
        {
            Character target = c;
            if (target == null)
                return;

            target.Opponent = c;

            //damage -= WornArmour(target);
            //if (target.Ward == true)
            //damage >>= 1;

            if (damage > 0)
            {
                target.HP -= damage;
                if (target.HP <= 0)
                {
                    if (target.IsPlayer)
                    {
                        manager.GetUI().Message(target.Name + " has been killed");
                    }
                    Death(target, c);
                    Experience(c, 1);
                }
                else if (target.IsPlayer)
                    manager.GetUI().Message(target.Name + " takes " + damage + " points of damage");
            }
        }

        //Checks if a character dies
        public void Death(Character target, Character c)
        {
            if (target.IsPlayer)
            {
                manager.ChangeGameState(UI.MenuState.Death);
            }
            else
            {
                manager.GetCharacterManager().DisposeCharacter(target);
                manager.SetCamera();
            }


        }
    }
}
