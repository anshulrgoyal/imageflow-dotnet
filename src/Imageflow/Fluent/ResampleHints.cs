﻿using System;

namespace Imageflow.Fluent
{
    public class ResampleHints
    {
        public SharpenWhen? SharpenWhen { get; set; }
        public ResampleWhen? ResampleWhen { get; set; }
        
        public ScalingFloatspace? InterpolationColorspace { get; set; }
        public InterpolationFilter? UpFilter { get; set; }
        public InterpolationFilter? DownFilter { get; set; }
        public float? SharpenPercent { get; set; }

        public ResampleHints(float? sharpenPercent, SharpenWhen? sharpenWhen, ResampleWhen? resampleWhen, InterpolationFilter? downFilter, InterpolationFilter? upFilter, ScalingFloatspace? interpolationColorspace)
        {
            SharpenPercent = sharpenPercent;
            DownFilter = downFilter;
            UpFilter = upFilter;
            InterpolationColorspace = interpolationColorspace;
            ResampleWhen = resampleWhen;
            SharpenWhen = sharpenWhen;
        }

        public ResampleHints()
        {

        }


        [Obsolete("Use SetSharpen instead")]
        public ResampleHints Sharpen(float? sharpenPercent, SharpenWhen? sharpenWhen)
            => SetSharpen(sharpenPercent, sharpenWhen);
        public ResampleHints SetSharpen(float? sharpenPercent, SharpenWhen? sharpenWhen)
        {
            SharpenPercent = sharpenPercent;
            SharpenWhen = sharpenWhen;
            return this;
        }

        public ResampleHints SetSharpenPercent(float? sharpenPercent)
        {
            SharpenPercent = sharpenPercent;
            return this;
        }
        public ResampleHints SetSharpenWhen(SharpenWhen? sharpenWhen)
        {
            SharpenWhen = sharpenWhen;
            return this;
        }
        
        [Obsolete("Use SetResampleFilters instead")]
        public ResampleHints ResampleFilter(InterpolationFilter? downFilter, InterpolationFilter? upFilter)
        {
            DownFilter = downFilter;
            UpFilter = upFilter;
            return this;
        }
        
        public ResampleHints SetResampleFilters(InterpolationFilter? downFilter, InterpolationFilter? upFilter)
        {
            DownFilter = downFilter;
            UpFilter = upFilter;
            return this;
        }
        
        public ResampleHints SetUpSamplingFilter( InterpolationFilter? upFilter)
        {
            UpFilter = upFilter;
            return this;
        }
        public ResampleHints SetDownSamplingFilter(InterpolationFilter? downFilter)
        {
            DownFilter = downFilter;
            return this;
        }

        [Obsolete("Use SetResampleWhen instead")]
        public ResampleHints Resample(ResampleWhen? resampleWhen)
        {

            ResampleWhen = resampleWhen;
            return this;
        }
        public ResampleHints SetResampleWhen(ResampleWhen? resampleWhen)
        {

            ResampleWhen = resampleWhen;
            return this;
        }
        
        [Obsolete("Use SetInterpolationColorspace instead")]
        public ResampleHints ResampleColorspace( ScalingFloatspace? interpolationColorspace)
        {
            InterpolationColorspace = interpolationColorspace;
            return this;
        }
        
        public ResampleHints SetInterpolationColorspace( ScalingFloatspace? interpolationColorspace)
        {
            InterpolationColorspace = interpolationColorspace;
            return this;
        }


        public object ToImageflowDynamic()
        {
            return new
            {
                sharpen_percent = SharpenPercent,
                down_filter = DownFilter?.ToString().ToLowerInvariant(),
                up_filter = UpFilter?.ToString().ToLowerInvariant(),
                scaling_colorspace = InterpolationColorspace?.ToString().ToLowerInvariant(),
                resample_when = ResampleWhen?.ToString().ToLowerInvariant(),
                sharpen_when = SharpenWhen?.ToString().ToLowerInvariant()
            };
        }
    }

}