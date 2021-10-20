[System.Serializable]
    public class NoiseChannelConfig
    {
        public TweenParameter frequency = new TweenParameter(fixedValue: 1f);
        public TweenParameter amplitude = new TweenParameter(fixedValue: 1f);
        public TweenParameter valueCenter = new TweenParameter(fixedValue: 0.5f);
        public float offset = 0f;
        public bool enable = false;
        public float multiplier=1;
    }




