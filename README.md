# Trap Runner

Trap Runner is a 2D Unity game where the player does not directly control the runner. Instead, the player places traps along the course to stop an automatically moving Bot before it reaches the finish line three times.

## Gameplay

The Bot automatically runs toward the finish line while the player places traps in available trap slots.

The player can use three trap types:

- **Spike Trap** – kills the Bot on contact.
- **Falling Platform** – drops after being placed.
- **Spring Trap** – launches the Bot upward.

The Bot also uses an obstacle sensor to automatically jump over normal obstacles.

## Scoring

The Bot earns:

- **10 points** each time it reaches the finish line.

The player earns points based on how many traps were used before killing the Bot:

- **1 trap used:** 30 points
- **2 traps used:** 20 points
- **3 traps used:** 10 points

Example:

If the Bot reaches the finish line twice and the player then kills it after using all three traps:

- Player: 10
- Bot: 20
- **Bot Wins**

## End Conditions

The game ends when either:

- The Bot is killed, or
- The Bot reaches the finish line **3 times**.

The final scores are then compared to determine the winner.

## Controls

- **1** – Select Spike Trap
- **2** – Select Falling Platform
- **3** – Select Spring Trap
- **Left Click** – Place selected trap
- **P** – Pause / Resume
- **R** – Restart game

## Built With

- Unity 6
- C#
- TextMeshPro
- Unity 2D Physics

## Project Structure

The main Unity project files are contained in:

- `Assets/`
- `Packages/`
- `ProjectSettings/`

## Author

Jason Ventura