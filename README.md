#ScorpionEngine

###Index
- [Getting started](#getting-started)
- [GameStates and GameObjects](#gamestates-and-gameobjects)
- [Assets](#assets)
- [Particle System](#particle-system)

###Getting started
In the Initialize method of your game class is where you initialize the engine (which I will be referring to as SE).
It needs the graphics device manager, content manager, and the name of the spritefont it will use as a console font. Leave this null if you don't wan't to use the game console.<br>
In your Update and Draw methods you need to respectively update and draw the SE too. The SE creates its own spritebatch, so you don't need to pass that as a parameter.
```
protected override void Initialize()
{
  SE.Initialize(graphics, Content, "consoleFont");
  base.Initialize();
}

protected override void Update(GameTime gameTime)
{
  SE.Update(gameTime);
  base.Update(gameTime);
}

protected override void Draw(GameTime gameTime)
{
  SE.Draw();
  base.Draw(gameTime);
}
```

###GameStates and GameObjects
By calling Update and Draw, the SE will update and draw all GameObjects in the current GameState. Objects in a gamestate other than the current one are not updated or drawn.

You can create your own gamestate classes by inheriting them from the GameState class. In the constructor you then need to call the base constructor with the name of the gamestate. This name will be used to switch between states.
```
//Note that GameState is part of the GameObjects namespace
using ScorpionEngine.GameObjects;

public class PlayingState : GameState
{
  public PlayingState() : base("playing")
  {
    //Constructor stuff
  }
}
```

Each GameObject has a couple of useful things:
- Name
- Position
- RelativePosition
- Parent
- Children

Let's start at the beginning: the name of a GameObject is how you find or remove GameObjects from a GameState. It's Position is the absolute position, that is the RelativePosition plus the RelativePosition of the parent (and the parents parent, etc). The RelativePosition is the position as compared to  its parent. You can only set a GameObjects RelativePosition, not its Position. The Parent is a GameObject, and the Children is a list of GameObjects. When adding a child, the parent will be set accordingly and vice versa.

###Assets
All of your games assets will be obtained and used through the Assets class. The supported types are:
- Texture2D
- SpriteFont
- Effect
- SoundEffect
- Song
- Video

There is no need to load anything before getting it, because everything that isn't loaded when getting it will get loaded and stored, so it only needs to be loaded once. However, for sounds and music it can be useful to load it beforehand as this may cause lag.
```
//Load the item (not necessary)
Assets.Load<Texture2D>("name");
//Get the item
Texture2D tex = Assets.Get<Texture2D>("name");
```

###Particle System
The SE has a built-in particle system. This consists of a ParticleEmitter, Particles and ParticleModifiers. However, you only need to use the ParticleEmitter and ParticleModifiers because the Particles are created by the emitter. The ParticleEmitter is also a GameObject, which means you can add it to the GameState and don't have to worry about updating or drawing it.
```
//Create a list of modifiers
List<ParticleModifier> modifiers = new List<ParticleModifier>
{
  new RandomSpawnSpeedModifier(2.0f),
  new ColorLerpModifier(Color.Red, Color.Yellow),
  new OutlineCircleModifier(50)
};
//Create an emitter
ParticleEmitter emitter = new ParticleEmitter("emitter", new Vector2(100), 1, Vector2.Zero, 0, 0, Assets.Get<Texture2D>("particle"), Color.White, 4000, 1, modifiers);
//Add the emitter to the gamestate
Add(emitter);
```
There are quite some different kinds of built-in particle modifiers, and the best way to find out about the emitter and modifiers is to play around with them and trying different things.

Namespaces:<br>
ScorpionEngine<br>
.GameObjects<br>
.GameObjects.MenuItems<br>
.ParticleSystem<br>
.Shapes


Classes by namespace:  
- ScorpionEngine:  
SE (The main class)  
Assets  
Benchmark  
Camera2D  
Extensions  
GameConsole  
Input  
SpriteSheet

- ScorpionEngine.GameObjects:  
GameObject  
GameState

- ScorpionEngine.GameObjects.MenuItems:  
Button  
Slider  
ToggleButton

- ScorpionEngine.ParticleSystem:  
Particle  
ParticleEmitter  
ParticleModifier

- ScorpionEngine.Shapes:  
Line
