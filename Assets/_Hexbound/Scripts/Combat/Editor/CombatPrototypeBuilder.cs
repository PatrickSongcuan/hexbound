using System.IO;
using Hexbound.Combat;
using Hexbound.UI;
using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

namespace Hexbound.EditorTools
{
    /// <summary>
    /// Hexbound → Build Combat Prototype. Creates the M1 data assets, prefabs and Run-scene objects.
    /// Safe to re-run: existing data assets and prefabs are reused (your tuning is kept),
    /// and the scene step is skipped if a "Player" object already exists.
    /// </summary>
    public static class CombatPrototypeBuilder
    {
        private const string Root = "Assets/_Hexbound";
        private const string DataFolder = Root + "/Data";
        private const string PrefabFolder = Root + "/Prefabs";
        private const string RunScenePath = Root + "/Scenes/Run.unity";

        private static readonly Color PlayerColor = new Color(0.30f, 0.60f, 1.00f);
        private static readonly Color EnemyColor = new Color(0.90f, 0.30f, 0.30f);
        private static readonly Color FireboltColor = new Color(1.00f, 0.60f, 0.10f);
        private static readonly Color FloorDotColor = new Color(1f, 1f, 1f, 0.08f);

        private static Sprite circle;
        private static Sprite square;

        [MenuItem("Hexbound/Build Combat Prototype")]
        public static void Build()
        {
            if (Resources.Load<TMP_Settings>("TMP Settings") == null)
            {
                Debug.LogWarning("[Hexbound] TextMeshPro essentials are missing. Click 'Import TMP Essentials', " +
                                 "close the window, then run Hexbound → Build Combat Prototype again.");
                if (!EditorApplication.ExecuteMenuItem("Window/TextMeshPro/Import TMP Essential Resources"))
                    Debug.LogWarning("[Hexbound] Open it manually: Window → TextMeshPro → Import TMP Essential Resources.");
                return;
            }

            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                return;

            circle = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Knob.psd");
            square = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd");

            Directory.CreateDirectory(DataFolder);
            Directory.CreateDirectory(PrefabFolder);

            // Open Run first so temporary prefab-building objects never touch another scene.
            var scene = EditorSceneManager.OpenScene(RunScenePath);

            var spell = LoadOrCreateAsset<SpellData>("Firebolt");
            var stats = LoadOrCreateAsset<PlayerStats>("PlayerStats");
            var enemyData = LoadOrCreateAsset<EnemyData>("EnemyData_Test");
            var feel = LoadOrCreateAsset<GameFeelData>("GameFeel");
            var wave = LoadOrCreateAsset<TestWaveData>("TestWave");

            var projectile = LoadOrCreatePrefab<Projectile>("Projectile_Firebolt", BuildProjectile);
            var enemy = LoadOrCreatePrefab<EnemyChaser>("Enemy_Test", go => BuildEnemy(go, enemyData));
            var damageNumber = LoadOrCreatePrefab<DamageNumber>("DamageNumber", BuildDamageNumber);

            if (spell.projectilePrefab == null)
            {
                spell.projectilePrefab = projectile;
                EditorUtility.SetDirty(spell);
            }
            if (wave.enemyPrefab == null)
            {
                wave.enemyPrefab = enemy;
                EditorUtility.SetDirty(wave);
            }
            AssetDatabase.SaveAssets();

            if (GameObject.Find("Player") != null)
            {
                Debug.Log("[Hexbound] Run scene already has a Player; scene step skipped. Assets/prefabs are up to date.");
                return;
            }

            BuildRunScene(stats, spell, feel, wave, damageNumber);
            EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.SaveScene(scene);
            Debug.Log("[Hexbound] Combat prototype built. Open Run.unity and press Play.");
        }

        // ---------- Prefabs ----------

        private static void BuildProjectile(GameObject go)
        {
            AddSprite(go, circle, FireboltColor, 0.35f, sortingOrder: 5);
            var body = go.AddComponent<Rigidbody2D>();
            body.bodyType = RigidbodyType2D.Kinematic;
            body.gravityScale = 0f;
            body.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            var col = go.AddComponent<CircleCollider2D>();
            col.isTrigger = true;
            col.radius = circle.bounds.extents.x;
            go.AddComponent<Projectile>();
        }

        private static void BuildEnemy(GameObject go, EnemyData data)
        {
            AddSprite(go, square, EnemyColor, 0.8f, sortingOrder: 1);
            var body = go.AddComponent<Rigidbody2D>();
            body.gravityScale = 0f;
            body.freezeRotation = true;
            body.interpolation = RigidbodyInterpolation2D.Interpolate;
            var col = go.AddComponent<BoxCollider2D>();
            col.size = square.bounds.size;
            var health = go.AddComponent<Health>();
            SetEnum(health, "team", (int)Team.Enemy);
            var chaser = go.AddComponent<EnemyChaser>();
            SetRef(chaser, "data", data);
        }

        private static void BuildDamageNumber(GameObject go)
        {
            var text = go.AddComponent<TextMeshPro>();
            text.text = "0";
            text.fontSize = 5f;
            text.fontStyle = FontStyles.Bold;
            text.alignment = TextAlignmentOptions.Center;
            text.sortingOrder = 100;
            text.rectTransform.sizeDelta = new Vector2(3f, 1f);
            go.AddComponent<DamageNumber>();
        }

        // ---------- Scene ----------

        private static void BuildRunScene(PlayerStats stats, SpellData spell, GameFeelData feel, TestWaveData wave,
            DamageNumber damageNumberPrefab)
        {
            // URP 2D lit sprites need a light; one global light = flat, full-bright look.
            if (Object.FindAnyObjectByType<Light2D>() == null)
            {
                var light = new GameObject("Global Light 2D").AddComponent<Light2D>();
                light.lightType = Light2D.LightType.Global;
            }

            BuildFloorDots();
            VirtualJoystick joystick = BuildJoystick();

            var player = new GameObject("Player");
            AddSprite(player, circle, PlayerColor, 0.9f, sortingOrder: 2);
            var body = player.AddComponent<Rigidbody2D>();
            body.gravityScale = 0f;
            body.freezeRotation = true;
            body.mass = 3f;
            body.interpolation = RigidbodyInterpolation2D.Interpolate;
            player.AddComponent<CircleCollider2D>().radius = circle.bounds.extents.x;
            SetEnum(player.AddComponent<Health>(), "team", (int)Team.Player);
            var controller = player.AddComponent<PlayerController>();
            SetRef(controller, "stats", stats);
            SetRef(controller, "joystick", joystick);
            SetRef(player.AddComponent<SpellCaster>(), "spell", spell);

            Camera cam = Camera.main;
            if (cam != null)
            {
                cam.orthographicSize = 8f;
                cam.backgroundColor = new Color(0.12f, 0.12f, 0.15f);
                var rig = cam.gameObject.AddComponent<CameraRig>();
                SetRef(rig, "target", player.transform);
                SetRef(rig, "feel", feel);
            }
            else
            {
                Debug.LogWarning("[Hexbound] No MainCamera-tagged camera in Run; add CameraRig to your camera manually.");
            }

            SetRef(new GameObject("TestEnemySpawner").AddComponent<TestEnemySpawner>(), "wave", wave);

            var feedback = new GameObject("Feedback");
            SetRef(feedback.AddComponent<HitStop>(), "feel", feel);
            var numbers = feedback.AddComponent<DamageNumbers>();
            SetRef(numbers, "prefab", damageNumberPrefab);
            SetRef(numbers, "feel", feel);
        }

        private static VirtualJoystick BuildJoystick()
        {
            var canvasGo = new GameObject("JoystickCanvas", typeof(Canvas), typeof(CanvasScaler));
            canvasGo.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
            var scaler = canvasGo.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1080f, 1920f);
            scaler.matchWidthOrHeight = 0.5f;

            RectTransform background = CreateUIImage("JoystickBase", canvasGo.transform, 260f, new Color(1f, 1f, 1f, 0.2f));
            CreateUIImage("JoystickKnob", background, 120f, new Color(1f, 1f, 1f, 0.6f));
            RectTransform knob = (RectTransform)background.Find("JoystickKnob");

            var joystick = canvasGo.AddComponent<VirtualJoystick>();
            SetRef(joystick, "background", background);
            SetRef(joystick, "knob", knob);
            return joystick;
        }

        private static RectTransform CreateUIImage(string name, Transform parent, float size, Color color)
        {
            var go = new GameObject(name, typeof(RectTransform), typeof(Image));
            var rt = (RectTransform)go.transform;
            rt.SetParent(parent, false);
            rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.5f);
            rt.sizeDelta = new Vector2(size, size);
            var image = go.GetComponent<Image>();
            image.sprite = circle;
            image.color = color;
            image.raycastTarget = false;
            return rt;
        }

        /// <summary>Faint dots on a grid so you can see movement while the camera follows.</summary>
        private static void BuildFloorDots()
        {
            var floor = new GameObject("Floor");
            const int half = 15;
            const float spacing = 2f;
            for (int x = -half; x <= half; x++)
            for (int y = -half; y <= half; y++)
            {
                var dot = new GameObject("Dot");
                dot.transform.SetParent(floor.transform, false);
                dot.transform.localPosition = new Vector3(x * spacing, y * spacing, 0f);
                AddSprite(dot, circle, FloorDotColor, 0.15f, sortingOrder: -10);
            }
        }

        // ---------- Helpers ----------

        private static void AddSprite(GameObject go, Sprite sprite, Color color, float worldDiameter, int sortingOrder)
        {
            var sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = sprite;
            sr.color = color;
            sr.sortingOrder = sortingOrder;
            go.transform.localScale = Vector3.one * (worldDiameter / sprite.bounds.size.x);
        }

        private static T LoadOrCreateAsset<T>(string name) where T : ScriptableObject
        {
            string path = $"{DataFolder}/{name}.asset";
            var asset = AssetDatabase.LoadAssetAtPath<T>(path);
            if (asset != null)
                return asset;

            asset = ScriptableObject.CreateInstance<T>();
            AssetDatabase.CreateAsset(asset, path);
            Debug.Log($"[Hexbound] Created {path}");
            return asset;
        }

        private static T LoadOrCreatePrefab<T>(string name, System.Action<GameObject> build) where T : Component
        {
            string path = $"{PrefabFolder}/{name}.prefab";
            var existing = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (existing != null)
                return existing.GetComponent<T>();

            var temp = new GameObject(name);
            build(temp);
            GameObject saved = PrefabUtility.SaveAsPrefabAsset(temp, path);
            Object.DestroyImmediate(temp);
            Debug.Log($"[Hexbound] Created {path}");
            return saved.GetComponent<T>();
        }

        // Private [SerializeField]s can't be assigned from outside, so go through SerializedObject
        // (the same path the Inspector uses).
        private static void SetRef(Object target, string field, Object value)
        {
            var so = new SerializedObject(target);
            so.FindProperty(field).objectReferenceValue = value;
            so.ApplyModifiedPropertiesWithoutUndo();
        }

        private static void SetEnum(Object target, string field, int index)
        {
            var so = new SerializedObject(target);
            so.FindProperty(field).enumValueIndex = index;
            so.ApplyModifiedPropertiesWithoutUndo();
        }
    }
}
