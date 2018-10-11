package jj47.cs262.calvin.edu.homework2;

import android.content.Context;
import android.support.annotation.NonNull;
import android.support.annotation.Nullable;
import android.support.v4.content.AsyncTaskLoader;

public class PlayerLoader extends AsyncTaskLoader<String> {

    private String mQueryString;

    /**
     * Method called when starting the loader.
     */
    @Override
    protected void onStartLoading() {
        super.onStartLoading();

        // Starts the loadinBackground() method.
        forceLoad();
    }

    /**
     * Constructor
     *
     * @param context application context.
     */
    public PlayerLoader(@NonNull Context context, String query) {
        super(context);

        this.mQueryString = query;
    }


    /**
     * Method performs task in background.
     *
     * @return query results.
     */
    @Nullable
    @Override
    public String loadInBackground() {

        // Call method to query specified URI.
        // Based on whether string is empty or contains a positive integer value.
        if(mQueryString.isEmpty()){
            return NetworkUtils.getPlayerListInfo(mQueryString);
        }
        else {
            return NetworkUtils.getPlayerIDInfo(mQueryString);
        }
    }
}
