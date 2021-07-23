using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Stazor.Themes.Helpers
{
    /// <summary>
    /// 例外をスローするためのヘルパークラスです。
    /// </summary>
    static class ThrowHelper
    {
        /// <summary>
        /// 新しい<see cref="ValidationException"/>例外をスローします。
        /// </summary>
        /// <param name="validationResults">検証結果</param>
        /// <exception cref="ValidationException">常にこの例外をスローします。</exception>
        [DebuggerHidden]
        [DoesNotReturn]
        public static void ThrowValidationException(IReadOnlyCollection<ValidationResult> validationResults)
        {
            var message = string.Join(
                Environment.NewLine,
                validationResults.Select(x => $"DataAnnotation validation failed for members {string.Join(", ", x.MemberNames)} with the error '{x.ErrorMessage}'"));
            throw new ValidationException(message);
        }
    }
}