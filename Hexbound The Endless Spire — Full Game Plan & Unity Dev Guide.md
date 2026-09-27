# Hexbound: The Endless Spire — Full Game Plan & Unity Dev Guide

Sep 27, 2026 · @Patrick

## 1. Game overview

Hexbound is a portrait, one-handed roguelite where a cursed witch climbs an endless procedural tower, stacking random spell upgrades into builds. Runs last 3–10 minutes; permanent unlocks make every death feel like progress. It is built solo in Unity with an agentic AI workflow, where you direct and learn while agents do the heavy lifting.

**Pitch:** "Vampire Survivors meets Hades, in your pocket."

**Design pillars**

- **One more run:** short runs, instant restart, a new unlock roughly every 2–3 runs early on.
- **Build-crafting:** 3-choice upgrades that combine into surprising synergies.
- **Readable chaos:** busy screens stay legible on a 6-inch phone.
- **Fair free-to-play:** money buys style and convenience, never power in competitive modes.

**Audience:** mid-core mobile players aged 16–35 who play in commutes and breaks. Genre references: Archero, Vampire Survivors, Slay the Spire, Hades.

**Platforms:** iOS and Android, portrait, offline-playable, online for dailies and leaderboards.

**Scope target for launch (v1.0):** 3 witches, 40 hexes, 3 biomes of 10 floors plus a boss each, endless mode, daily run, cosmetic shop, battle pass season 1. Everything else is post-launch live-ops.

## 2. Core loop and controls

The game runs on three nested loops: fight a floor (60–90 s), build a run (10–30 floors), grow the account (weeks).

**Moment to moment (per floor)**

1. Floor spawns: arena, hazards, enemy waves.
2. Witch moves with a virtual joystick anywhere on the lower half of the screen.
3. Spells auto-cast at the nearest enemy when standing still (Archero rule), rewarding stop-and-go positioning.
4. One active ability (dash or ultimate) on a thumb button with a cooldown.
5. Floor clears → a door opens → 3 hex choices → next floor.

**Per run**

- Every 5th floor: shop (spend run gold) or a random event (shrine, cursed chest, NPC).
- Every 10th floor: biome boss, then pick a relic (a rare, rule-bending upgrade).
- Death or victory → results screen: floors reached, build summary, Soul Ash earned, one-tap restart.

**Per account (meta)**

- Spend Soul Ash in the Coven (hub) on permanent upgrades, new witches and new hexes added to the pool.
- Daily run, weekly mutator and season pass give reasons to return.

**Controls rules**

- Everything reachable by one thumb in portrait; no UI in the top 20% except HUD readouts.
- Joystick is "floating" (appears where the thumb lands).
- Haptics on hit, level-up and boss kill (toggle in settings).
- Pause on app backgrounding, always.

## 3. Content

Launch content is 3 witches, 40 hexes, 12 enemy types and 3 bosses; everything is data, so adding more later needs no new code.

**Witches (3 at launch, 2 post-launch)**

| Witch | Playstyle | Starting spell | Passive | Unlock |
| --- | --- | --- | --- | --- |
| Ember | Aggressive DPS | Firebolt (burn) | Kills heal 1 HP | Start |
| Mirelle | Control | Frost shard (slow) | Slowed enemies take +20% damage | 150 Soul Ash |
| Nyx | Summoner | Shadow wisp (orbits) | Summons inherit 50% of hexes | Beat biome 2 |
| Briar | Tank / thorns | Thorn burst | Reflect 30% melee damage | Post-launch |
| Vex | Glass cannon | Chaos orb | +50% damage, -50% max HP | Post-launch |

**Hexes (40): four schools, 10 each**

- **Fire:** burn, explosion on kill, heat stacks, fire trail.
- **Frost:** slow, freeze, shatter, ice armor.
- **Storm:** chain lightning, speed, crits, static charge.
- **Shadow:** summons, lifesteal, curses, projectile pierce.
- Rarity tiers: Common (60%), Rare (30%), Legendary (10%, only after floor 5).

**Synergy examples (the "aha" moments)**

1. **Shatter + Chain Lightning:** frozen enemies hit by lightning explode into ice shards.
2. **Fire Trail + Speed Boots:** faster movement paints more burning ground.
3. **Lifesteal + Glass Cannon:** high risk, high sustain.
4. **Split Shot + Pierce + Summons:** wisps fire copies of your projectiles.
5. **Heat Stacks + Freeze:** "Thermal Shock" deals triple damage when an enemy flips from hot to frozen.
6. Owning 3 hexes from one school unlocks a **School Mastery** hex offered next floor.

**Enemies (4 per biome)**

| Role | Behavior | Example |
| --- | --- | --- |
| Swarmer | Rushes in groups, low HP | Candle imps |
| Ranged | Keeps distance, fires patterns | Hex archers |
| Tank | Slow, high HP, shields allies | Stone golems |
| Disruptor | Teleports, drops hazards | Mirror wraiths |

**Biomes and bosses**

| Floors | Biome | Hazard | Boss |
| --- | --- | --- | --- |
| 1–10 | Candlewood Cellar | Falling wax | The Wick King (phases lit and unlit) |
| 11–20 | Frozen Library | Sliding shelves | The Archivist (summons book swarms) |
| 21–30 | Storm Belfry | Lightning strikes | The Bellwarden (sound-wave rings) |
| 31+ | The Endless (remixed) | Mixed + mutators | Rotating remixed bosses, scaling forever |

## 4. Replayability systems

Six stacked systems keep runs fresh past 500 hours: seeded generation, build variety, curses, dailies, weekly mutators and endless mode.

**Procedural floors**

- Every run has one integer seed; all randomness flows from it (enables daily runs and bug reproduction).
- Floors are built from hand-made room templates (about 30 at launch) plus random hazards, enemy mixes and reward types.
- A "director" budgets difficulty points per floor and spends them on enemies, so difficulty scales smoothly.

**Curses (Heat-style difficulty)**

- 10 stackable curses unlock after the first clear, such as enemies +25% speed, no healing shrines, elites every floor.
- Each curse adds a Soul Ash multiplier (for example +15%).
- Cosmetic rewards for clearing at curse levels 5, 10, 15 and 20.

**Daily and weekly**

- **Daily Hex:** one shared seed and a fixed witch; one ranked attempt a day on a global leaderboard.
- **Weekly Mutator:** a rule change for everyone, such as "gravity wells" or "all hexes are Shadow".

**Endless mode**

- Unlocks after beating floor 30; enemy HP and damage scale about +8% per floor with no cap.
- Personal best depth is the main bragging stat and has its own leaderboard.

**Unlock pacing**

- Something new every 2–3 runs for the first 50 hours: hexes, relics, witches, room types, lore.
- 30 lore fragments hidden in rare events reveal who cursed the witches.
- Rare events (1–3% chance): a wandering merchant, a mirror room with your past run's ghost, a golden hex.

## 5. Meta progression and economy

The economy has three currencies with strict jobs: Soul Ash (earned, progression), Gold (per run only) and Moonstones (premium, cosmetics and convenience).

| Currency | Earned by | Spent on | Purchasable |
| --- | --- | --- | --- |
| Gold | Kills, chests in a run | Run shop; resets on death | No |
| Soul Ash | End of every run (about 10 per floor reached) | Permanent upgrades, witches, hex unlocks | No |
| Moonstones | Battle pass, achievements, small daily trickle | Skins, pass premium track, extra rerolls | Yes |

**Coven upgrades (permanent, Soul Ash)**

- 8 upgrade trees such as Max HP, Starting reroll, Luck (rarity odds), Revive once per run.
- Costs rise per level (for example 20, 40, 80, 150, 250) so the first week feels fast and later levels feel earned.
- Target: a new player clears biome 1 around run 8–12 and floor 30 around run 40–60.

**Design rules**

- Soul Ash is never sold; power is earned by playing.
- Every run pays at least 10 Soul Ash, so short or bad runs still count.
- Log every currency source and sink in analytics from day one to tune pacing.

## 6. Monetization plan

Revenue comes from cosmetics, a seasonal pass, opt-in rewarded ads and one ad-removal purchase; nothing sold affects leaderboard fairness. Prices below are USD starting points to test.

| Product | Price | What the player gets | Notes |
| --- | --- | --- | --- |
| Supporter Pack | $4.99 once | No interstitial ads, ad rewards granted instantly, 1 exclusive skin | Best-converting item in most F2P games |
| Starter Bundle | $1.99 once | 1 skin, 300 Moonstones | Offered after run 3, available 72 h |
| Battle Pass | $6.99 per 8-week season | 40 tiers of cosmetics, Moonstones, lore | Free track always exists |
| Moonstone packs | $0.99 – $49.99 | 100 – 6,500 Moonstones | Bonus % grows with size |
| Skins | 400 – 1,200 Moonstones | Witch skins, spell VFX, familiars, tower themes | Rotating weekly shop |

**Rewarded ads (opt-in only)**

- Revive once per run, double end-of-run Soul Ash, one free reroll per run.
- Cap at about 6 rewarded ads a day; never forced mid-run.
- Interstitials (if used at all) only on the results screen, at most one per 3 runs, never in the first 10 runs.

**Never sell**

- Soul Ash, permanent stat upgrades, hexes, or anything that affects Daily Hex or leaderboards.
- Randomized paid boxes: avoid them to stay clear of loot box rules in several countries and store disclosure requirements.

**Benchmarks to aim for (genre ranges, approximate)**

- Payer conversion 2–4%; D1 retention 40%+, D7 15%+, D30 5%+.
- Revenue split target: roughly 50% ads, 50% IAP early; IAP share grows as the pass and shop mature.

## 7. Unity setup and agentic toolchain

Use the latest Unity 6 LTS with the Universal 2D template, Git from minute one, and Claude Code working directly in the project folder while you keep the Unity Editor open.

**Install**

1. Unity Hub, then the newest Unity 6 LTS with Android and iOS build support modules (iOS builds need a Mac with Xcode).
2. An IDE: VS Code with the Unity extension, or JetBrains Rider.
3. Git plus Git LFS; a private GitHub repo. Commit after every working step so you can always roll back an agent's change.
4. Claude Code (terminal, VS Code extension, or the Code tab in Claude Desktop), signed in with your Claude account.
5. Optional: a Unity MCP bridge (search GitHub for "Unity MCP") so Claude Code can read the Console, create GameObjects and run play mode. Start without it; add it at Milestone 2 once you understand the basics.

**Packages (Package Manager)**

- Input System, URP 2D, TextMeshPro, Addressables, Cinemachine, Test Framework.
- Later: Unity IAP, an ad mediation SDK (Unity LevelPlay or AdMob), Unity Gaming Services (Analytics, Cloud Save, Remote Config, Leaderboards).
- Tweening: DOTween (free on the Asset Store) or PrimeTween.

**Project folders**

```
Assets/_Hexbound/
  Art/        Audio/      Data/        (ScriptableObject assets)
  Prefabs/    Scenes/     UI/          (UXML/USS or uGUI prefabs)
  Scripts/
    Core/     (GameManager, events, save, RNG)
    Combat/   (player, spells, enemies, damage)
    Run/      (floor gen, director, hex choice)
    Meta/     (currencies, upgrades, unlocks)
    UI/       Services/ (ads, IAP, analytics)
  Tests/
```

**The learning-while-agentic contract (put this in CLAUDE.md)**

Claude Code reads `CLAUDE.md` at the start of every session, so it is where you tell the agent to teach you as it builds.

```
# Hexbound — agent rules
- Unity 6 LTS, C#, URP 2D, portrait 1080x1920, new Input System.
- Data-driven: gameplay values live in ScriptableObjects, never hard-coded.
- Small steps: one feature per task, compilable after every step, commit message suggested at the end.
- Teach mode: after each task, explain in under 150 words what you built, the Unity concept behind it,
  and one thing I should try changing myself to learn it.
- Every 3rd task, leave one small TODO(me) for me to implement, with a hint, instead of writing it.
- Never add packages or assets without asking. Never touch ProjectSettings unless asked.
- When you can't see the Unity Editor, give me exact click-by-click Editor steps.
```

**Your weekly learning rhythm**

- Let the agent build, then read every changed file before committing.
- Do the TODO(me) tasks by hand; ask Claude to review, not rewrite.
- Once a week, rebuild one small system from memory in a scratch scene.

## 8. Claude Code models and effort levels (Pro plan)

On Pro, keep Opus 5.5 at its default medium effort for most work, drop to Sonnet 5 for routine grind, and save xhigh, max and ultracode for rare, hard problems, because higher settings burn your usage limits much faster. This assumes the Pro plan (there is no "Plus" plan); on Max you can afford higher effort more often.

**What each dial does**

- **Model** = how smart the brain is. **Effort** = how much work it does per request (thinking, tool calls, double-checking).
- Pro's default is Opus 5.5 at medium effort. Sonnet 5 defaults to high.
- Levels: low → medium → high → xhigh → max. **Ultracode** is not a level: it runs xhigh and fans substantive tasks out to multiple agents, so it is the most usage-hungry option.
- **Fable 5.1** (`/model fable`) is the most capable model for long autonomous work; on some plans it bills to paid usage credits and asks for consent first.
- Typing `ultrathink` in one prompt asks for deeper reasoning on that turn only.

**Which setting for which Hexbound task**

| Task | Model | Effort | Why |
| --- | --- | --- | --- |
| Architecture, save system, procedural generator design | Opus 5.5 (plan mode) | high | Mistakes here are expensive to undo |
| New gameplay feature (spell, enemy AI, hex effect) | Opus 5.5 | medium (default) | Best quality per token |
| Bulk content: 40 hex ScriptableObjects, JSON data, UI wiring | Sonnet 5 | medium | Repetitive, well-specified |
| Rename, small fix, comments, commit messages | Haiku 4.5 or Sonnet 5 | low | Trivial tasks |
| Explaining code to you (teach mode) | Sonnet 5 | medium | Clear explanations are cheap |
| Stubborn bug after 2 failed attempts | Opus 5.5 | xhigh, or add `ultrathink` | Needs deeper investigation |
| Performance profiling, memory leaks on device | Opus 5.5 | high | Multi-file reasoning |
| Big cross-cutting refactor, once or twice per project | Opus 5.5 | ultracode | Parallel agents; expect heavy usage |
| Whole-milestone autonomous build (optional) | Fable 5.1 | default | Only if you accept usage-credit billing |

**Commands you'll use**

```
/model                 # picker; arrow keys adjust effort; Enter saves, s = this session only
/model sonnet          # switch model
/effort medium         # set effort (low | medium | high | xhigh | max | ultracode)
/effort auto           # back to the model's default
claude --model opusplan   # Opus plans in plan mode, Sonnet executes: great Pro-friendly default
/status                # see model, account, usage
```

**Pro-plan habits that stretch your limits**

- Start big features in plan mode, approve the plan, then execute; rework is the biggest token waste.
- Keep one feature per session and start fresh sessions often; long sessions re-read more context.
- Put subagents (test runners, file searches) on Sonnet or Haiku via the `model` field in their definitions.
- If Claude misses a file or skips tests, raise effort; if it had full context and still got it wrong, switch to a stronger model.

Source: [Claude Code model configuration](https://code.claude.com/docs/en/model-config) and [choosing a model and effort level](https://claude.com/blog/claude-model-and-effort-level-in-claude-code). Models and defaults change; recheck `/model` when you update Claude Code.

## 9. Unity architecture

The architecture rests on four ideas: ScriptableObjects hold all content, one seeded RNG drives every random roll, systems talk through events instead of direct references, and a single save service owns persistent data. Learn these four and you understand the whole codebase.

**Core systems**

| System | Responsibility | Key classes |
| --- | --- | --- |
| Game flow | Hub → run → results state machine | `GameManager`, `GameState` |
| Run | Seed, floor count, current hexes, gold | `RunState`, `RunController` |
| Floor generation | Pick room template, spend difficulty budget | `FloorGenerator`, `Director` |
| Combat | Movement, auto-cast, damage, status effects | `PlayerController`, `SpellCaster`, `Health`, `StatusEffect` |
| Hexes | Apply modifiers and triggers to stats and spells | `HexData`, `HexEffect`, `StatBlock` |
| Meta | Soul Ash, upgrades, unlocks | `MetaProgress`, `UpgradeData` |
| Services | Save, ads, IAP, analytics behind interfaces | `ISaveService`, `IAdService`, `IStoreService` |

**Content as data: a hex**

```csharp
[CreateAssetMenu(menuName = "Hexbound/Hex")]
public class HexData : ScriptableObject
{
    public string id;
    public string displayName;
    [TextArea] public string description;
    public HexSchool school;          // Fire, Frost, Storm, Shadow
    public Rarity rarity;
    public Sprite icon;
    public int minFloor = 1;
    [SerializeReference] public List<HexEffect> effects = new();
}

[System.Serializable]
public abstract class HexEffect
{
    public abstract void Apply(PlayerContext ctx);
    public virtual void Remove(PlayerContext ctx) { }
}

[System.Serializable]
public class StatModifierEffect : HexEffect
{
    public StatType stat; public float percent;
    public override void Apply(PlayerContext ctx) => ctx.Stats.AddPercent(stat, percent);
}
```

**One seeded RNG for the whole run**

```csharp
public class RunRandom
{
    readonly System.Random _rng;
    public int Seed { get; }
    public RunRandom(int seed) { Seed = seed; _rng = new System.Random(seed); }
    public int Range(int min, int max) => _rng.Next(min, max);
    public float Value() => (float)_rng.NextDouble();
    public T Pick<T>(IList<T> list) => list[_rng.Next(list.Count)];
}
// Daily run: seed = int.Parse(DateTime.UtcNow.ToString("yyyyMMdd"))
```

**Events instead of references**

```csharp
public static class GameEvents
{
    public static event System.Action<Enemy> EnemyKilled;
    public static event System.Action<int> FloorCleared;
    public static event System.Action<RunResult> RunEnded;
    public static void RaiseEnemyKilled(Enemy e) => EnemyKilled?.Invoke(e);
    public static void RaiseFloorCleared(int f) => FloorCleared?.Invoke(f);
    public static void RaiseRunEnded(RunResult r) => RunEnded?.Invoke(r);
}
// Hex "explode on kill" subscribes to EnemyKilled; analytics subscribes to RunEnded.
```

**Other rules**

- Object pooling for projectiles, enemies, damage numbers and VFX from day one (`UnityEngine.Pool.ObjectPool`).
- Save data is one JSON file via `JsonUtility`, versioned, written on run end and app pause; mirrored to Cloud Save later.
- Services sit behind interfaces with a fake implementation for the Editor, so you can test ads and IAP without a phone.

## 10. Development milestones with agent prompts

Eleven milestones take you from empty project to soft launch in about 24 weeks part-time (roughly 15–20 hours a week). Each has a goal, the model and effort to use, copy-paste prompts, a learning checkpoint, and a "done when" test. Never start the next milestone until the current one's test passes and is committed.

**How to run every prompt:** open Claude Code in the project root, check the model and effort with `/model`, use plan mode for anything marked (plan), read the plan, then approve.

### M0 — Project foundation (week 1)

**Model:** Opus 5.5, medium. **Goal:** clean project, repo, CLAUDE.md, folder structure, empty scenes.

```
(plan) Read CLAUDE.md. Set up the Hexbound Unity project foundation:
- Create the folder structure under Assets/_Hexbound as described in CLAUDE.md.
- Create scenes: Boot, Hub, Run. Boot loads Hub.
- Add a .gitignore for Unity and a .gitattributes for Git LFS (png, wav, ogg, psd, fbx).
- Create GameManager (singleton, DontDestroyOnLoad) with a GameState enum: Boot, Hub, Run, Results.
- Set portrait 1080x1920 and list the exact Editor clicks I must do for Player Settings.
Explain each file in teach mode.
```

**Learn:** what a scene, prefab, MonoBehaviour and the Unity lifecycle (Awake, Start, Update) are. **Done when:** Boot → Hub loads in play mode; first commit pushed.

### M1 — The "fun test" prototype (weeks 2–3)

**Model:** Opus 5.5, medium. **Goal:** grey boxes, but moving and shooting already feels good.

```
(plan) Build the core combat prototype in the Run scene using placeholder shapes:
1. PlayerController with a floating virtual joystick (new Input System, touch + mouse for Editor).
2. SpellCaster: when the player stops moving, auto-fire Firebolt at the nearest enemy every 0.6 s.
3. Projectiles use UnityEngine.Pool.ObjectPool.
4. Health component shared by player and enemies; damage numbers pop up.
5. One test enemy that walks toward the player.
6. A SpellData ScriptableObject holding damage, fire rate, speed, so I can tune in the Inspector.
Add screen shake and hit-stop (50 ms) on hit so it feels juicy.
```

Follow-up prompt after playing it:

```
The movement feels [floaty / stiff / slow]. Suggest 3 tuning changes with exact values, explain the
"game feel" principle behind each, and expose them all in the Inspector so I can test live.
```

**Learn:** Input System actions, Rigidbody2D vs transform movement, object pooling. Do the TODO(me): add a dash ability yourself. **Done when:** you enjoy dodging and killing 20 test enemies for 60 seconds.

### M2 — Enemies and procedural floors (weeks 4–5)

**Model:** Opus 5.5, high for the generator; Sonnet 5, medium for enemy variants.

```
(plan, high effort) Design and implement procedural floors:
- RunRandom seeded RNG class (see architecture in CLAUDE.md); every random call in a run uses it.
- RoomTemplate ScriptableObject: arena bounds, spawn points, hazard slots.
- Director: given floor number, it gets a difficulty budget (base 10, +15% per floor) and buys enemies
  from an EnemyData list by cost until the budget is spent, in 2–4 waves.
- FloorGenerator picks a template, places hazards, runs the Director.
- Floor cleared when all waves are dead; raise GameEvents.FloorCleared.
Add an Editor debug window: enter a seed and floor number, press Generate, see the result.
```

```
Using the EnemyData and enemy base class, create the 4 Candlewood Cellar enemies: Candle Imp (swarmer),
Hex Archer (ranged, keeps 5 units away), Stone Golem (tank, shields nearby allies), Mirror Wraith
(teleports behind the player every 4 s). Keep behaviors as small state machines. Placeholder sprites.
```

**Learn:** ScriptableObjects, state machines, why seeds make bugs reproducible. **Done when:** the same seed always gives the same floor, and 10 floors in a row feel different.

### M3 — Hexes and synergies (weeks 6–7)

**Model:** Opus 5.5, medium for the system; Sonnet 5, medium for bulk content.

```
(plan) Implement the hex system from the design doc section 3:
- HexData ScriptableObject with [SerializeReference] List<HexEffect>.
- Effect types: StatModifier, OnHitTrigger, OnKillTrigger, ProjectileModifier (split, pierce, bounce),
  StatusApplier (burn, slow, freeze), Summon.
- HexPool picks 3 unique offers weighted by rarity and minFloor, using RunRandom.
- Hex choice UI after each floor (3 cards, tap to pick, one reroll).
- Synergy checks: owning 3 hexes of one school adds its Mastery hex to the next offer.
Write EditMode unit tests for HexPool weighting and for stat stacking.
```

```
(Sonnet 5) Create all 40 HexData assets from this table: [paste your hex list]. Generate them with an
Editor script that reads a CSV in Assets/_Hexbound/Data/hexes.csv, so I can rebalance by editing the CSV.
```

**Learn:** polymorphism with SerializeReference, unit testing in Unity, CSV-driven content. **Done when:** at least 5 synergies from the design doc work, and a run with 10 hexes still runs at 60 fps.

### M4 — Full run loop, bosses, meta and save (weeks 8–10)

**Model:** Opus 5.5, high for save and meta; medium for bosses.

```
(plan, high effort) Complete the run loop:
- RunState holds seed, floor, hexes, gold; RunController drives floors, shop every 5th floor,
  boss every 10th, and relic choice after bosses.
- Results screen: floors, build summary, Soul Ash earned (10 per floor + curse multiplier), one-tap restart.
- MetaProgress: Soul Ash balance, 8 upgrade trees from UpgradeData assets, witch and hex unlocks.
- SaveService behind ISaveService: versioned JSON in persistentDataPath, save on run end and
  OnApplicationPause, migration hook for future versions. Never lose data if the app is killed.
- Hub scene with an upgrade screen.
```

```
Create the Wick King boss with 2 phases: phase 1 (lit) fires rotating flame spirals; below 50% HP he
blows out his candle and phase 2 (unlit) darkens the arena except a light radius around the player,
with dash attacks telegraphed by a 0.8 s red line. Build it as a reusable BossPhase system for later bosses.
```

**Learn:** serialization, data migration, state machines at scale. **Done when:** you can play 5 full runs, quit mid-run, and your Soul Ash and upgrades survive.

### M5 — UI/UX pass (weeks 11–12)

**Model:** Opus 5.5, medium for flows; Sonnet 5, medium for wiring screens. **Goal:** every screen readable one-handed, with a clear first-time flow.

Step 1, design before code (in Claude chat or Claude Design, not Claude Code):

```
Act as a senior mobile UX designer. Design the screen flow and wireframes for Hexbound, a portrait
one-handed roguelite: Boot, Hub (Coven), Witch select, Run HUD, Hex choice, Shop, Pause, Results,
Upgrades, Store, Battle Pass, Settings. Rules: thumb zone in the bottom 60%, HUD only at the top,
max 2 taps from Hub to a new run, 48 dp minimum touch targets, colorblind-safe school colors.
Show each screen as a mockup and list the navigation flow between them.
```

Step 2, build it:

```
(plan) Implement the UI from docs/ui-spec.md using uGUI + TextMeshPro:
- A UIManager with a screen stack (push, pop, back button support on Android).
- Safe-area handling for notches.
- Reusable components: PrimaryButton (press scale + haptic), CurrencyLabel (animated count-up),
  HexCard (rarity frame, school icon, description).
- Transitions with DOTween: 0.2 s fades and slides; no transition longer than 0.35 s.
- First-time user flow: runs 1–3 show one tooltip each (move, auto-cast, pick a hex). Skippable.
```

**Learn:** anchors and layout groups, canvas scaling, why screen stacks beat scene loads for menus. **Done when:** 3 friends can start a run and pick a hex without you explaining anything.

### M6 — Art and VFX (weeks 12–14, overlaps M5)

**Model:** Sonnet 5, medium for import tooling; Opus 5.5, medium for shaders. **Goal:** replace every grey box with a consistent style. Section 11 covers the asset-generation prompts.

```
Create an Editor import pipeline: any PNG dropped into Art/Sprites gets Sprite mode, 100 PPU,
point or bilinear filter per folder rule, and is added to the right SpriteAtlas (Characters,
Enemies, Hexes, UI, Environment). Add a validator that warns about textures over 2048 px.
```

```
Create URP 2D effects: a hit-flash shader (white flash 0.08 s), a dissolve shader for enemy deaths,
burn / freeze / shock status tints, and particle prefabs for Firebolt, Frost Shard, Chain Lightning,
Shadow Wisp. Keep particles under 50 per effect for mobile. Explain how Shader Graph nodes work as you go.
```

**Learn:** sprite atlases and draw calls, Shader Graph basics, the 2D Renderer and lights. **Done when:** a screenshot looks like one game, not a collage.

### M7 — Audio (weeks 14–15)

**Model:** Sonnet 5, medium. **Goal:** every action has sound feedback; music changes per biome and boss.

```
(plan) Build the audio system:
- AudioManager with an AudioMixer: Master, Music, SFX, UI groups; volume sliders saved in settings.
- SoundData ScriptableObject: list of clips, volume, pitch range (random 0.9–1.1 to avoid repetition),
  max simultaneous instances, cooldown.
- Pooled AudioSources for SFX. Music crossfades (1.5 s) between Hub, each biome, and boss tracks.
- Hook sounds to GameEvents so gameplay code never calls audio directly.
- Respect the device mute switch and pause audio when the app is backgrounded.
```

**Learn:** audio mixers, ducking (lower music during boss roars), why pitch variation matters. **Done when:** you can play with your eyes closed and still tell a hit, a kill, a level-up and low HP apart.

### M8 — Replayability systems (weeks 16–17)

**Model:** Opus 5.5, high for dailies and leaderboards; medium for curses and mutators.

```
(plan, high effort) Add replayability from design doc section 4:
- Curses: 10 CurseData assets, a curse selection screen after first clear, Soul Ash multiplier.
- Daily Hex: seed from UTC date, fixed witch, one ranked attempt per day, results submitted to
  Unity Gaming Services Leaderboards (use a fake service in the Editor).
- Weekly Mutators: MutatorData assets fetched from Remote Config, fallback list in the build.
- Endless mode after floor 30: +8% HP and damage per floor, remixed biome and boss rotation.
- Rare events with 1–3% chance using RunRandom.
```

**Learn:** Remote Config, server time vs device time (cheating), async/await with services. **Done when:** two phones get the identical Daily Hex floor 1, and scores appear on the leaderboard.

### M9 — Monetization and services (weeks 18–19)

**Model:** Opus 5.5, high (money and store rules are unforgiving).

```
(plan, high effort) Integrate monetization from design doc section 6, all behind interfaces with
Editor fakes:
- IStoreService with Unity IAP: Supporter Pack (non-consumable), Starter Bundle, Battle Pass season,
  Moonstone packs (consumables). Restore Purchases button. Receipt validation hook.
- IAdService with [LevelPlay or AdMob]: rewarded ads only for revive, double Soul Ash, reroll;
  daily cap 6; Supporter Pack grants rewards without ads.
- Battle Pass: 40 tiers, XP from runs, free and premium tracks, season end date from Remote Config.
- Analytics events: run_start, run_end (floor, witch, hexes), purchase, ad_watched, currency_earned,
  currency_spent, tutorial_step.
- Consent: GDPR/CMP prompt and iOS App Tracking Transparency before any ad SDK initializes.
List every dashboard setup step I must do manually in Google Play Console, App Store Connect, and UGS.
```

**Learn:** consumable vs non-consumable purchases, sandbox testing, privacy consent. **Done when:** sandbox purchases work on real Android and iOS devices, and a restore on a fresh install returns the Supporter Pack.

### M10 — Polish, performance and soft launch (weeks 20–24)

**Model:** Opus 5.5, high for profiling; Sonnet 5 for store copy and checklists.

```
(high effort) I attached a Unity Profiler capture from a low-end Android phone during floor 25.
Find the top 5 performance costs (CPU, GC allocations, draw calls, overdraw) and fix them one at a
time, re-explaining what each profiler column means. Target: 60 fps, no GC spikes over 1 ms mid-run.
```

```
Write the Google Play and App Store listings for Hexbound: title, 30-char subtitle, short and long
description, 10 keywords, and captions for 6 screenshots. Also write a privacy policy outline
based on the analytics and ads SDKs we use.
```

- Soft launch on Android in 1–2 smaller markets for 4–6 weeks; watch D1, D7, crash rate and funnel drop-off.
- Fix the biggest drop-off, then launch globally on both stores.

**Done when:** crash-free rate above 99.5%, D1 retention near 40%, and you've shipped at least 2 updates from player feedback.

## 11. AI pipeline for art, UI/UX and sound

The key to AI art that looks like one game is locking a style first, reusing one style suffix in every prompt, and hand-cleaning every asset before import. Claude plans and writes prompts; an image or audio generator makes the files; you curate.

**Tool roles**

| Job | Tool | Your part |
| --- | --- | --- |
| Style bible, prompts, UI specs | Claude (chat) | Approve direction |
| Screen mockups to react to | Claude Design or a Claude artifact | Pick layouts |
| Sprites, icons, backgrounds | An image generator (you already have Everygen connected here; Midjourney, Scenario or similar also work) | Pick best of 4, clean edges |
| Cleanup, palette fixing, slicing | Aseprite, Krita or Photoshop | Remove artifacts, match palette |
| Character animation | Unity 2D Animation (bone rigging) plus tweened squash-and-stretch | Rig once per witch |
| SFX | An AI sound-effect generator (Everygen's audio generation works), jsfxr or ChipTone for crisp retro sounds | Trim, normalize |
| Music | An AI music tool whose license allows commercial games, or a composer for the main theme | Check the license |

**Step 1: style bible (Claude chat, Opus 5.5)**

```
Create an art bible for Hexbound, a cozy-spooky witchcraft roguelite for phones. Define: art style
(hand-painted 2D, thick 3 px dark outlines, top-down 3/4 view), a 24-color master palette with hex
codes, one accent color per spell school (Fire orange, Frost cyan, Storm yellow, Shadow violet),
silhouette rules so enemies read at 64 px, lighting direction (top-left), and a reusable
"STYLE SUFFIX" sentence I will append to every image prompt.
```

**Step 2: asset prompts (always end with your STYLE SUFFIX)**

```
Witch character sprite, Ember, young fire witch with a scorched pointed hat and ember-glowing cloak,
full body, facing down-right, idle pose, centered, plain flat magenta background for easy removal.
[STYLE SUFFIX]
```

```
Game icon for a spell upgrade called "Chain Lightning", single symbol, bold readable shape at
96x96, yellow accent, dark circular frame, no text. [STYLE SUFFIX]
```

```
Top-down arena floor tile set, candle-lit stone cellar, dripping wax, 4 seamless variations,
low contrast so characters stand out. [STYLE SUFFIX]
```

```
Mobile game UI kit: primary button, secondary button, panel frame, card frame in 3 rarities
(grey, blue, gold), currency icons for Gold, Soul Ash, Moonstone. Flat background. [STYLE SUFFIX]
```

**Step 3: sound prompts**

```
Write a sound design list for Hexbound: every gameplay, UI and ambient event with a one-line
description, length in ms, and priority. Then write generator prompts for the top 30, for example
"short magical fire whoosh, 300 ms, bright attack, soft tail, no reverb".
```

```
Music brief for the Candlewood Cellar biome: 90 BPM, loopable 2-minute track, music box, pizzicato
strings, low choir pads, playful-spooky, builds intensity in the second half, seamless loop point.
Write a matching boss track brief at 140 BPM with the same motif.
```

**Rules that save you later**

- Keep a `licenses.csv` of every asset: tool, date, prompt, license terms. Stores and publishers may ask.
- Generate on a flat background color, then remove it; transparent-background output is often messy.
- Target sizes: characters 256 px, enemies 128–256 px, icons 256 px (displayed at 96), backgrounds 1080×1920.
- Export audio as 44.1 kHz WAV, import as Vorbis for music and ADPCM for short SFX.
- Test every asset on a real phone at arm's length before calling it done.

## 12. Live-ops after launch

After launch, ship on a fixed rhythm: a content drop every 4 weeks and a new battle pass season every 8, driven by analytics rather than guesses. Remote Config lets you tune without app updates.

**Cadence**

- Weekly: new mutator, shop rotation, balance tweaks via Remote Config.
- Every 4 weeks: content drop (5–8 hexes, 1 room set, 1 rare event, cosmetics).
- Every 8 weeks: new season with a themed pass; every second season adds a witch or biome.

**Metrics to review every Monday**

- Retention D1, D7, D30; median runs per day; average floor reached per witch.
- Hex pick rate vs win rate: nerf hexes picked and winning too often, buff ones nobody picks.
- Funnel: install → tutorial done → first boss → first purchase.
- Revenue per daily user, ad views per user, pass conversion.

**Agent prompt for weekly balance**

```
(Sonnet 5, medium) Here is last week's analytics export as CSV: [attach]. Find the 5 most over- and
under-performing hexes by pick rate vs win rate, propose number changes for each in hexes.csv, and
predict the effect. Do not change code, only data.
```

## 13. Testing and publishing checklist

Tick these off before soft launch; most store rejections and 1-star reviews come from items on this list.

**Testing**

- [ ] EditMode tests for hex pool, stat stacking, save migration and seeded generation
- [ ] PlayMode smoke test: start run, clear floor, pick hex, die, results, back to hub
- [ ] Tested on a low-end Android phone (2–3 GB RAM) and an older iPhone
- [ ] 60 fps on floor 25 with a full build; no GC spikes over 1 ms mid-run
- [ ] App killed mid-run, reopened: no data lost
- [ ] Airplane mode: game fully playable offline, daily and store fail gracefully

**Store and legal**

- [ ] Google Play developer account and Apple Developer Program membership
- [ ] Privacy policy URL, data safety form (Play), privacy nutrition labels (App Store)
- [ ] Age rating questionnaires completed; no random paid boxes
- [ ] GDPR consent and iOS App Tracking Transparency before ads initialize
- [ ] Restore Purchases works; all IAP tested in sandbox
- [ ] Asset `licenses.csv` complete for every AI-generated or third-party asset

**Release**

- [ ] Android App Bundle (AAB), IL2CPP, ARM64; iOS build archived from Xcode
- [ ] Crash reporting on (Unity Cloud Diagnostics or Firebase Crashlytics)
- [ ] Store listing: icon, 6 screenshots, 30 s trailer, localized for at least English
- [ ] Soft launch plan and success targets written down before you press publish
