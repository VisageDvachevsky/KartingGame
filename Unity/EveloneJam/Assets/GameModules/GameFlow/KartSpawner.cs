using Project.Kart;
using Project.RoadGeneration;
using UnityEngine;
using Zenject;
using System.Collections.Generic;
using Project.Laps;
using System;
using Random = UnityEngine.Random;

namespace Project.GameFlow
{
    public class KartSpawner : MonoBehaviour
    {
        [SerializeField] private KartController _playerKartPrefab;
        [SerializeField] private KartController[] _aiKartPrefabs;

        private PlayerKartProvider _kartProvider;
        private IRoadProvider _roadProvider;
        private MapSettings _mapSettings;
        private SkinSettings _skinSettings;
        private DiContainer _container;

        [Inject]
        private void Construct(DiContainer container,
            PlayerKartProvider kartProvider,
            IRoadProvider roadProvider,
            MapSettings mapSettings,
            SkinSettings skinSettings)
        {
            _container = container;
            _kartProvider = kartProvider;
            _roadProvider = roadProvider;
            _mapSettings = mapSettings;
            _skinSettings = skinSettings;
        }

        public void RespawnKart()
        {
            if (_kartProvider.Kart)
                Destroy(_kartProvider.Kart.gameObject);

            IReadOnlyList<Transform> spawnpoints = _roadProvider.KartSpawnpoints;

            int playerSpawnpointIndex = Random.Range(0, spawnpoints.Count);
            SpawnPlayerKart(spawnpoints, playerSpawnpointIndex);
            SpawnBots(spawnpoints, playerSpawnpointIndex);
        }

        private void SpawnBots(IReadOnlyList<Transform> spawnpoints, int playerSpawnpointIndex)
        {
            int spawned = 0;
            for (int i = 0; i < spawnpoints.Count; i++)
            {
                if (spawned >= _mapSettings.BotCount) break;
                if (i == playerSpawnpointIndex) continue;

                KartController kart = SpawnKartAt(spawnpoints[i], _aiKartPrefabs[Random.Range(0, _aiKartPrefabs.Length)]);

                var allColors = Enum.GetValues(typeof(SkinStore.KartColor));
                SkinStore.KartColor color = (SkinStore.KartColor)allColors.GetValue(Random.Range(0, allColors.Length));
                var allCharacters = Enum.GetValues(typeof(SkinStore.KartCharacter));
                SkinStore.KartCharacter character = (SkinStore.KartCharacter)allCharacters.GetValue(Random.Range(0, allCharacters.Length));

                kart.KartSkin.SetSkin(color, character);

                spawned++;
            }
        }

        private void SpawnPlayerKart(IReadOnlyList<Transform> spawnpoints, int playerSpawnpointIndex)
        {
            KartController kart = SpawnKartAt(spawnpoints[playerSpawnpointIndex], _playerKartPrefab);
            kart.KartSkin.SetSkin(_skinSettings.CurrentColor, _skinSettings.CurrentCharacter);
            kart.IsPlayer = true;
            _kartProvider.SetKart(kart);
        }

        private KartController SpawnKartAt(Transform spawnpoint, KartController prefab)
        {
            KartController kart = _container.InstantiatePrefabForComponent<KartController>(prefab);
            kart.transform.position = spawnpoint.position;
            kart.transform.rotation = spawnpoint.rotation;

            return kart;
        }
    }
}