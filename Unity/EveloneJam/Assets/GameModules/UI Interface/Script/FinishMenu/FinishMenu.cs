using DG.Tweening;
using Project.Kart;
using Project.Laps;
using Project.RoadGeneration;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

public class FinishMenu : MonoBehaviour
{
    [SerializeField] private GameObject _finishMenuRoot;
    [SerializeField] private TextMeshProUGUI _playerPlaceText;
    [SerializeField] private Transform _cup1;
    [SerializeField] private Transform _cup2;
    [SerializeField] private Transform _cup3;

    private ScoreSystem _scoreSystem;
    private PlayerKartProvider _kartProvider;

    [Inject]
    private void Construct(ScoreSystem scoreSystem, PlayerKartProvider kartProvider)
    {
        _scoreSystem = scoreSystem;
        _kartProvider = kartProvider;
    }

    private void Start()
    {
        _finishMenuRoot.SetActive(false);
    }

    public void Show()
    {
        CheckpointCounter player = _kartProvider.Kart.CheckpointCounter;
        IReadOnlyList<ScoreSystem.ScoreData> scores = _scoreSystem.ScoreDatas;

        int racersCount = scores.Count;
        int place = -1;
        for (int i = 0; i < scores.Count; i++)
        {
            if (scores[i].CheckpointCounter == player)
            {
                place = i + 1;
                break;
            }
        }

        _cup1.localScale = Vector3.zero;
        _cup2.localScale = Vector3.zero;
        _cup3.localScale = Vector3.zero;
        _finishMenuRoot.transform.localScale = Vector3.zero;
        _finishMenuRoot.SetActive(true);
        _finishMenuRoot.transform.DOScale(Vector3.one, 0.5f);
        _playerPlaceText.text = $"{place} место";

        StartCoroutine(CupsRoutine(place, racersCount));
    }

    private IEnumerator CupsRoutine(int place, int racersCount)
    {
        if (place <= racersCount * 0.75f)
        {
            _cup1.DOScale(Vector3.one, 0.75f);
            yield return new WaitForSeconds(0.75f);
        }
        yield return null;
        if (place <= racersCount * 0.5f)
        {
            _cup2.DOScale(Vector3.one, 0.75f);
            yield return new WaitForSeconds(0.75f);
        }
        yield return null;
        if (place == 1)
        {
            _cup3.DOScale(Vector3.one, 0.75f);
            yield return new WaitForSeconds(0.75f);
        }
    }
}
