package jj47.cs262.calvin.edu.lab05;

import android.content.Context;
import android.net.ConnectivityManager;
import android.net.NetworkInfo;
import android.support.annotation.NonNull;
import android.support.annotation.Nullable;
import android.support.v4.app.LoaderManager;
import android.support.v4.content.Loader;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.view.inputmethod.InputMethodManager;
import android.widget.EditText;
import android.widget.TextView;

import org.json.JSONArray;
import org.json.JSONObject;

import java.util.Objects;

public class MainActivity extends AppCompatActivity implements LoaderManager.LoaderCallbacks<String> {

    private static final String LOG_TAG = MainActivity.class.getSimpleName();

    private EditText editText;
    private TextView author;
    private TextView title;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        // Find components.
        editText = findViewById(R.id.book_Input);
        author = findViewById(R.id.authorText);
        title = findViewById(R.id.titleText);

        // Reconnect to the loader if one exists already, upon device config change.
        if(getSupportLoaderManager().getLoader(0)!=null){
            getSupportLoaderManager().initLoader(0,null,this);
        }
    }

    /**
     * Method begins Google BooksAPI query process upon button click.
     *
     * @param view view component
     */
    public void searchBooks(View view) {

        // Get the query string.
        String queryString = editText.getText().toString();

        // Close keyboard after hitting search query button.
        InputMethodManager inputManager = (InputMethodManager)
                getSystemService(Context.INPUT_METHOD_SERVICE);
        if (inputManager != null) {
            inputManager.hideSoftInputFromWindow(Objects.requireNonNull(getCurrentFocus()).getWindowToken(),
                    InputMethodManager.HIDE_NOT_ALWAYS);
        }

        // Initialize network info components.
        ConnectivityManager connMgr = (ConnectivityManager)
                getSystemService(Context.CONNECTIVITY_SERVICE);

        NetworkInfo networkInfo = null;

        if (connMgr != null) {
            networkInfo = connMgr.getActiveNetworkInfo();
        }

        // Check connection is available, we are connected, and query string is not empty.
        if (networkInfo != null && networkInfo.isConnected() && queryString.length() != 0) {

            // FIXME: Fetchbook.java is now deprecated.  Can remove.
            // Initiate the search.
            //new Fetchbook(title, author).execute(queryString);

            // Refactored to user AsyncTaskLoader via BookLoader.java
            Bundle queryBundle = new Bundle();
            queryBundle.putString("queryString", queryString);
            getSupportLoaderManager().restartLoader(0, queryBundle,this);

            // Indicate to user query is in process.
            author.setText("");
            title.setText(R.string.loading_in_process);
        } else {
            // User didn't enter anything to search for.
            if (queryString.length() == 0) {
                author.setText("");
                title.setText(R.string.no_search_term);

            // There is no available connection.
            } else {
                author.setText("");
                title.setText(R.string.no_connection);
            }
        }
    }

    /**
     * Method called when load is instantiated.
     *
     * @param i
     * @param bundle contains data
     * @return query results
     */
    @NonNull
    @Override
    public Loader<String> onCreateLoader(int i, @Nullable Bundle bundle) {
            return new BookLoader(this, Objects.requireNonNull(bundle).getString("queryString"));
    }

    /**
     * Method called when loader task is finished.  Add code to update UI with results.
     *
     * @param loader loader object.
     * @param s
     */
    @Override
    public void onLoadFinished(@NonNull Loader<String> loader, String s) {

        // obtain the JSON array of results items
        try {
            JSONObject jsonObject = new JSONObject(s);
            JSONArray itemsArray = jsonObject.getJSONArray("items");

            //Iterate through the results
            for (int i = 0; i < itemsArray.length(); i++) {
                JSONObject book = itemsArray.getJSONObject(i); //Get the current item
                String titles = null;
                String authors = null;
                JSONObject volumeInfo = book.getJSONObject("volumeInfo");

                try {
                    titles = volumeInfo.getString("title");
                    authors = volumeInfo.getString("authors");
                } catch (Exception e) {
                    e.printStackTrace();
                }

                //If both a title and author exist, update the TextViews and return
                if (titles != null && authors != null) {
                    title.setText(titles);
                    author.setText(authors);
                    return;
                }
            }

            title.setText(R.string.no_results_found);
            author.setText("");

        } catch (Exception ex){

            title.setText(R.string.no_results_found);
            author.setText("");
            ex.printStackTrace();

        } finally{
            Log.e(LOG_TAG,"Finished");
            return;
        }
    }

    /**
     * Method called to clean up any remaining resources.
     *
     * @param loader loader object.
     */
    @Override
    public void onLoaderReset(@NonNull Loader<String> loader) {

    }
}
