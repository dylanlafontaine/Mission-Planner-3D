﻿/*     INFINITY CODE 2013-2019      */
/*   http://www.infinity-code.com   */

/// <summary>
/// Interface for a plugin that supports state saving
/// </summary>
public interface IOnlineMapsSavableComponent
{
    /// <summary>
    /// Returns an array of items to save
    /// </summary>
    /// <returns>Array of items to save</returns>
    OnlineMapsSavableItem[] GetSavableItems();
}