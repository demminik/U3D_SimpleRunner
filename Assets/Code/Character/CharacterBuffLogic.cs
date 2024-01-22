using Runner.Gameplay.Core.Buffs;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Runner.Gameplay.Core.Characters {

    public class CharacterBuffLogic : MonoBehaviour, ICharacterBuffTargetWriter, ICharacterBuffTargetReader {

        private struct ActiveBuffInfo {

            public ICharacterBuff Buff;
            public float BuffEndTime;
        }

        private Dictionary<ECharacterBuffType, Func<ICharacterBuff, bool>> _buffProcessors = new Dictionary<ECharacterBuffType, Func<ICharacterBuff, bool>>();
        private List<ActiveBuffInfo> _activeBuffs = new List<ActiveBuffInfo>();
        private HashSet<ECharacterBuffType> _activeBuffKeys = new HashSet<ECharacterBuffType>();

        private RunnerGameplayFasade _gameplayFasade;

        public event Action<ECharacterBuffType> OnBuffApplied;
        public event Action<ECharacterBuffType> OnBuffEnded;

        private void OnDestroy() {
            Dispose();
        }

        public void Initialize(RunnerGameplayFasade gameplayFasade) {
            _gameplayFasade = gameplayFasade;

            FillBuffProcesors();
        }

        public void Dispose() {
            _buffProcessors.Clear();
            _activeBuffs.Clear();
            _activeBuffKeys.Clear();
        }

        public void Tick(float deltaTime) {
            for (int i = _activeBuffs.Count - 1; i >= 0; i--) {
                var item = _activeBuffs[i];
                if (_gameplayFasade.GameTime.CurrentTime >= item.BuffEndTime) {
                    TryRemoveActiveBuffRecord(item.Buff.Type);
                    ProcessBuffEnded(item.Buff.Type);
                }
            }
        }

        private void FillBuffProcesors() {
            _buffProcessors.Clear();
            var buffTypes = (ECharacterBuffType[])Enum.GetValues(typeof(ECharacterBuffType));
            foreach (var item in buffTypes) {
                _buffProcessors[item] = TryApplyBuffDefault;
            }
            _buffProcessors[ECharacterBuffType.None] = TryApplyInvaildBuff;
        }

        public void Apply(ICharacterBuff buff) {
            if (buff == null) {
                UnityEngine.Debug.LogError("Buff application failed: buff is NULL");
                return;
            }
            if(!_buffProcessors.TryGetValue(buff.Type, out var buffProcessor)) {
                UnityEngine.Debug.LogError("Buff application failed: no suitable buff processor found");
            }

            TryEndExistingBuff(buff.Type);
            if (buffProcessor.Invoke(buff)) {
                AddActiveBuffRecord(buff);
                ProcessBuffApplied(buff);
            }
        }

        public void Remove(ECharacterBuffType buffType) {
            TryEndExistingBuff(buffType);
        }

        private bool TryApplyBuffDefault(ICharacterBuff buff) {
            return true;
        }

        private bool TryApplyInvaildBuff(ICharacterBuff buff) {
            UnityEngine.Debug.LogError($"Buff {buff.Type} application failed: invalid buff");
            return false;
        }

        private bool TryEndExistingBuff(ECharacterBuffType buffType) {
            if (TryRemoveActiveBuffRecord(buffType)) {
                ProcessBuffEnded(buffType);
                return true;
            }

            return false;
        }

        private void AddActiveBuffRecord(ICharacterBuff buff) {
            var buffType = buff.Type;

            _activeBuffKeys.Add(buffType);
            _activeBuffs.Add(new ActiveBuffInfo() {
                Buff = buff,
                BuffEndTime = _gameplayFasade.GameTime.CurrentTime + buff.Duration,
            });
        }

        private bool TryRemoveActiveBuffRecord(ECharacterBuffType buffType) {
            if (!_activeBuffKeys.Contains(buffType)) {
                return false;
            }

            var buffIndex = -1;
            for (int i = _activeBuffs.Count - 1; i >= 0; i--) {
                var item = _activeBuffs[i];
                if (item.Buff.Type == buffType) {
                    buffIndex = i;
                    break;
                }
            }

            if(buffIndex >= 0) {
                TryRemoveActiveBuffRecord(buffType, buffIndex);
            } else {
                UnityEngine.Debug.LogError($"Failed to remove buff record: buff type is found in active buffs but no actual buff record found", this);
            }

            return false;
        }

        private bool TryRemoveActiveBuffRecord(ECharacterBuffType buffType, int buffIndex) {
            _activeBuffKeys.Remove(buffType);
            _activeBuffs.RemoveAt(buffIndex);
            return true;
        }

        private void ProcessBuffApplied(ICharacterBuff buff) {
            OnBuffApplied?.Invoke(buff.Type);
        }

        private void ProcessBuffEnded(ECharacterBuffType buffType) {
            OnBuffEnded?.Invoke(buffType);
        }

        public ICharacterBuff GetActiveBuff(ECharacterBuffType buffType) {
            if (!_activeBuffKeys.Contains(buffType)) {
                return null;
            }

            for (int i = _activeBuffs.Count - 1; i >= 0; i--) {
                var item = _activeBuffs[i];
                if (item.Buff.Type == buffType) {
                    return item.Buff;
                }
            }

            return null;
        }

    }
}