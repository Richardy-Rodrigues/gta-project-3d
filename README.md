# 🎮 GTA Project 3D — FPS (Unity 6)

Jogo de tiro em primeira pessoa (FPS) construído sobre o template **Unity FPS Microgame**,
usando **Unity 6** e o **Universal Render Pipeline (URP)**. O objetivo é eliminar todos os
inimigos da fase, com sistema de pontuação e itens coletáveis de velocidade.

![Gameplay](docs/images/gameplay.png)

---

## 📑 Índice
- [Requisitos](#-requisitos)
- [Como rodar](#-como-rodar)
- [Como jogar](#-como-jogar)
- [Controles](#-controles)
- [Funcionalidades adicionadas](#-funcionalidades-adicionadas)
  - [Sistema de Pontuação](#1-sistema-de-pontuação)
  - [Pickup de Velocidade](#2-pickup-de-velocidade)
  - [Pickup de Escudo](#3-pickup-de-escudo)
- [Estrutura do projeto](#-estrutura-do-projeto)
- [Créditos](#-créditos)

---

## ✅ Requisitos

| Item | Versão |
|------|--------|
| Unity | **6000.4.0f1** (Unity 6) |
| Render Pipeline | Universal RP (URP) |
| Pacotes principais | Input System, AI Navigation (NavMesh), ProBuilder |

> Recomendado abrir o projeto pelo **Unity Hub** usando exatamente a versão acima para
> evitar reimportações ou incompatibilidades.

---

## 🚀 Como rodar

```bash
git clone git@github.com:Richardy-Rodrigues/gta-project-3d.git
```

1. Abra o **Unity Hub** → **Add** → selecione a pasta do projeto.
2. Abra o projeto com a versão **6000.4.0f1**.
3. No Editor, abra a cena `Assets/FPS/Scenes/MainScene.unity`.
4. Clique em **▶ Play**.

> Você também pode começar pela cena `Assets/FPS/Scenes/IntroMenu.unity` para ver o menu
> inicial.

---

## 🕹️ Como jogar

- **Objetivo:** eliminar **todos os inimigos** da fase. Ao limpar a área, você vai para a
  tela de vitória (`WinScene`) com o seu **placar final**.
- Se a sua vida chegar a zero (ou você cair no vazio), vai para a tela de derrota
  (`LoseScene`).
- Pegue **pickups** espalhados pela fase: vida, munição, **boost de velocidade** e
  **escudo** (invencibilidade temporária).

---

## 🎮 Controles

| Ação | Teclado / Mouse | Gamepad |
|------|-----------------|---------|
| Mover | `W` `A` `S` `D` / Setas | Left Stick |
| Olhar / virar câmera | Mouse | Right Stick |
| Atirar | Botão esquerdo do mouse | RT (gatilho direito) |
| Mirar (zoom) | Botão direito do mouse | LT (gatilho esquerdo) |
| Correr | `Left Shift` | L3 (apertar stick esq.) |
| Pular | `Espaço` | A (botão sul) |
| Agachar | `C` | — |
| Recarregar | `R` | X (botão oeste) |
| Trocar de arma | Scroll do mouse / `Q` · `E` | D-Pad |
| Selecionar arma | Teclas `1`–`9` | — |
| Pausar / Opções | `Tab` | — |

---

## 🧩 Funcionalidades adicionadas

Estas duas funcionalidades foram desenvolvidas sobre o template, reaproveitando a
arquitetura orientada a eventos do projeto (`EventManager` / `Events`) e a classe base
`Pickup`.

### 1. Sistema de Pontuação

Concede pontos ao jogador a cada inimigo eliminado, exibe um contador no HUD durante o jogo
e mostra o **placar final** na tela de vitória.

**Como funciona:**
- `ScoreManager` escuta o evento `EnemyKillEvent` (disparado quando um inimigo morre) e
  acumula os pontos.
- `ScoreCounter` lê a pontuação e atualiza o texto no HUD (`GameHUD.prefab`).
- `FinalScoreDisplay` mostra o total na `WinScene` (a pontuação é guardada num campo
  `static` que sobrevive à troca de cena).

**Configuração na Unity:**
- Componente **ScoreManager** adicionado ao `GameManager` da cena.
- **ScoreCounter** num texto (TextMeshPro) dentro do `GameHUD.prefab`.
- **FinalScoreDisplay** num texto na `WinScene`.

![Sistema de Pontuação no HUD](docs/images/score-hud.png)

<details>
<summary><strong>📄 ScoreManager.cs</strong></summary>

```csharp
using UnityEngine;

namespace Unity.FPS.Game
{
    // Tracks the player's score by listening to enemy kill events.
    // Add this component to a manager GameObject in the gameplay scene.
    public class ScoreManager : MonoBehaviour
    {
        [Tooltip("Points awarded for each enemy killed")]
        public int PointsPerEnemy = 100;

        public int CurrentScore { get; private set; }

        // Kept across scene loads so the WinScene can display the final score.
        public static int LastRunScore;

        void OnEnable()
        {
            EventManager.AddListener<EnemyKillEvent>(OnEnemyKill);
        }

        void OnDisable()
        {
            EventManager.RemoveListener<EnemyKillEvent>(OnEnemyKill);
        }

        void OnEnemyKill(EnemyKillEvent evt)
        {
            CurrentScore += PointsPerEnemy;
            LastRunScore = CurrentScore;
        }
    }
}
```
</details>

<details>
<summary><strong>📄 ScoreCounter.cs</strong> (HUD)</summary>

```csharp
using TMPro;
using Unity.FPS.Game;
using UnityEngine;

namespace Unity.FPS.UI
{
    public class ScoreCounter : MonoBehaviour
    {
        [Header("Score")] [Tooltip("Text component for displaying the current score")]
        public TextMeshProUGUI ScoreText;

        ScoreManager m_ScoreManager;

        void Awake()
        {
            m_ScoreManager = FindAnyObjectByType<ScoreManager>();
            DebugUtility.HandleErrorIfNullFindObject<ScoreManager, ScoreCounter>(m_ScoreManager, this);
        }

        void Update()
        {
            ScoreText.text = "Score: " + m_ScoreManager.CurrentScore;
        }
    }
}
```
</details>

<details>
<summary><strong>📄 FinalScoreDisplay.cs</strong> (WinScene)</summary>

```csharp
using TMPro;
using Unity.FPS.Game;
using UnityEngine;

namespace Unity.FPS.UI
{
    // Displays the score from the last run. Use this on the WinScene.
    public class FinalScoreDisplay : MonoBehaviour
    {
        [Header("Score")] [Tooltip("Text component for displaying the final score")]
        public TextMeshProUGUI ScoreText;

        void Start()
        {
            ScoreText.text = "Pontuação: " + ScoreManager.LastRunScore;
        }
    }
}
```
</details>

---

### 2. Pickup de Velocidade

Um item coletável que dá ao jogador um **aumento temporário de velocidade** de movimento.

**Como funciona:**
- `SpeedBoostPickup` herda da classe base `Pickup` (mesmo padrão do `HealthPickup`). Ao ser
  coletado, chama o boost no jogador e se autodestrói.
- A lógica do tempo vive em `PlayerSpeedBoost`, um componente **no jogador** (como o
  `Jetpack`), para que o efeito sobreviva à destruição do pickup. Ele multiplica a
  velocidade (`MaxSpeedOnGround`) por alguns segundos e depois restaura.
- Pegar dois pickups seguidos **renova a duração** em vez de acumular velocidade.

**Configuração na Unity:**
- Prefab `Pickup_SpeedBoost` (duplicado do `Pickup_Health`) com o componente
  **SpeedBoostPickup**.
- Campos configuráveis no Inspector: **Speed Multiplier** (padrão `1.5`) e **Duration**
  (padrão `5s`).
- O componente `PlayerSpeedBoost` é adicionado automaticamente ao jogador na primeira coleta.

![Pickup de Velocidade](docs/images/speedboost.png)

<details>
<summary><strong>📄 PlayerSpeedBoost.cs</strong> (no jogador)</summary>

```csharp
using System.Collections;
using Unity.FPS.Game;
using UnityEngine;

namespace Unity.FPS.Gameplay
{
    // Applies a temporary ground-speed boost to the player.
    // Lives on the player (like Jetpack) so the boost survives the pickup being destroyed.
    // Added automatically to the player by SpeedBoostPickup when first collected.
    public class PlayerSpeedBoost : MonoBehaviour
    {
        PlayerCharacterController m_Controller;
        float m_BaseSpeed;
        Coroutine m_BoostRoutine;

        void Awake()
        {
            m_Controller = GetComponent<PlayerCharacterController>();
            DebugUtility.HandleErrorIfNullGetComponent<PlayerCharacterController, PlayerSpeedBoost>(
                m_Controller, this, gameObject);

            m_BaseSpeed = m_Controller.MaxSpeedOnGround;
        }

        public void ApplyBoost(float multiplier, float duration)
        {
            // Restart from the base speed so overlapping boosts refresh the timer
            // instead of stacking the multiplier.
            if (m_BoostRoutine != null)
                StopCoroutine(m_BoostRoutine);

            m_BoostRoutine = StartCoroutine(BoostRoutine(multiplier, duration));
        }

        IEnumerator BoostRoutine(float multiplier, float duration)
        {
            m_Controller.MaxSpeedOnGround = m_BaseSpeed * multiplier;
            yield return new WaitForSeconds(duration);
            m_Controller.MaxSpeedOnGround = m_BaseSpeed;
            m_BoostRoutine = null;
        }
    }
}
```
</details>

<details>
<summary><strong>📄 SpeedBoostPickup.cs</strong> (no prefab do pickup)</summary>

```csharp
using UnityEngine;

namespace Unity.FPS.Gameplay
{
    public class SpeedBoostPickup : Pickup
    {
        [Header("Parameters")]
        [Tooltip("How much the player's ground speed is multiplied during the boost")]
        public float SpeedMultiplier = 1.5f;

        [Tooltip("How long the speed boost lasts, in seconds")]
        public float Duration = 5f;

        protected override void OnPicked(PlayerCharacterController player)
        {
            var speedBoost = player.GetComponent<PlayerSpeedBoost>();
            if (!speedBoost)
                speedBoost = player.gameObject.AddComponent<PlayerSpeedBoost>();

            speedBoost.ApplyBoost(SpeedMultiplier, Duration);
            PlayPickupFeedback();
            Destroy(gameObject);
        }
    }
}
```
</details>

---

### 3. Pickup de Escudo

Um item coletável que dá ao jogador **invencibilidade temporária** (escudo) por alguns
segundos.

**Como funciona:**
- `ShieldPickup` herda da classe base `Pickup`. Ao ser coletado, ativa o escudo no jogador
  e se autodestrói.
- A lógica do tempo vive em `PlayerShield`, um componente **no jogador** (mesmo padrão do
  boost de velocidade), para que o efeito sobreviva à destruição do pickup. Ele liga
  `Health.Invincible = true` por alguns segundos e depois desliga.
- Enquanto o escudo está ativo, o método `TakeDamage` do `Health` ignora todo o dano
  recebido. Pegar dois escudos seguidos **renova a duração** em vez de encerrar antes.

**Configuração na Unity:**
- Prefab `Pickup_Shield` (duplicado do `Pickup_Health`) com o componente **ShieldPickup**.
- Campo configurável no Inspector: **Duration** (padrão `5s`).
- O componente `PlayerShield` é adicionado automaticamente ao jogador na primeira coleta.

![Pickup de Escudo](docs/images/shield.png)

<details>
<summary><strong>📄 PlayerShield.cs</strong> (no jogador)</summary>

```csharp
using System.Collections;
using Unity.FPS.Game;
using UnityEngine;

namespace Unity.FPS.Gameplay
{
    // Grants the player temporary invincibility (a shield).
    // Lives on the player so the effect survives the pickup being destroyed.
    // Added automatically to the player by ShieldPickup when first collected.
    public class PlayerShield : MonoBehaviour
    {
        Health m_Health;
        Coroutine m_ShieldRoutine;

        void Awake()
        {
            m_Health = GetComponent<Health>();
            DebugUtility.HandleErrorIfNullGetComponent<Health, PlayerShield>(m_Health, this, gameObject);
        }

        public void ActivateShield(float duration)
        {
            // Restart the timer so overlapping pickups refresh the duration
            // instead of ending the shield early.
            if (m_ShieldRoutine != null)
                StopCoroutine(m_ShieldRoutine);

            m_ShieldRoutine = StartCoroutine(ShieldRoutine(duration));
        }

        IEnumerator ShieldRoutine(float duration)
        {
            m_Health.Invincible = true;
            yield return new WaitForSeconds(duration);
            m_Health.Invincible = false;
            m_ShieldRoutine = null;
        }
    }
}
```
</details>

<details>
<summary><strong>📄 ShieldPickup.cs</strong> (no prefab do pickup)</summary>

```csharp
using UnityEngine;

namespace Unity.FPS.Gameplay
{
    public class ShieldPickup : Pickup
    {
        [Header("Parameters")]
        [Tooltip("How long the invincibility shield lasts, in seconds")]
        public float Duration = 5f;

        protected override void OnPicked(PlayerCharacterController player)
        {
            var shield = player.GetComponent<PlayerShield>();
            if (!shield)
                shield = player.gameObject.AddComponent<PlayerShield>();

            shield.ActivateShield(Duration);
            PlayPickupFeedback();
            Destroy(gameObject);
        }
    }
}
```
</details>

---

## 📂 Estrutura do projeto

```
Assets/FPS/
├── Scenes/          # IntroMenu, MainScene, SecondaryScene, WinScene, LoseScene
├── Prefabs/         # Player, inimigos, pickups (inclui Pickup_SpeedBoost), HUD...
└── Scripts/
    ├── AI/          # Inimigos: EnemyController, EnemyMobile, EnemyTurret, detecção...
    ├── Game/        # Núcleo: Events, EventManager, managers (inclui ScoreManager)
    ├── Gameplay/    # Jogador, armas, pickups (Speed Boost e Shield + componentes do player)
    └── UI/          # HUD e telas (inclui ScoreCounter, FinalScoreDisplay)
```

---

## 🙏 Créditos

- **Base:** [Unity FPS Microgame](https://learn.unity.com/project/fps-template) (Unity Technologies).
- **Funcionalidades novas** (Sistema de Pontuação, Pickup de Velocidade e Pickup de Escudo):
  Richardy Rodrigues.
