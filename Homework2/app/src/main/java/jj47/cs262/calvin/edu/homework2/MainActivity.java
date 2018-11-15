package jj47.cs262.calvin.edu.homework2;

import android.annotation.SuppressLint;
import android.content.Context;
import android.content.Intent;
import android.net.ConnectivityManager;
import android.net.NetworkInfo;
import android.os.Bundle;
import android.support.annotation.NonNull;
import android.support.annotation.Nullable;
import android.support.v4.app.LoaderManager;
import android.support.v4.content.Loader;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.Toolbar;
import android.util.Log;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.view.inputmethod.InputMethodManager;
import android.widget.EditText;
import android.widget.TextView;
import android.widget.Toast;

import org.json.JSONArray;
import org.json.JSONObject;

import java.util.Objects;

/**
 * Class creates a UI for user interaction by which the user can retrieve a list of players or a
 * specific player via his/her ID.
 */
public class MainActivity extends AppCompatActivity implements LoaderManager.LoaderCallbacks<String> {

    // Private class members.
    private static final String LOG_TAG = MainActivity.class.getSimpleName();

    private EditText editTextQueryString;
    private TextView textViewSearchResults;

    private String queryString;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        // Setup action bar.
        Toolbar myToolbar = findViewById(R.id.my_toolbar);
        setSupportActionBar(myToolbar);

        // Find components.
        editTextQueryString = findViewById(R.id.search_string);
        textViewSearchResults = findViewById(R.id.search_results);

        // Reconnect to the loader if one exists already, upon device config change.
        if (getSupportLoaderManager().getLoader(0) != null) {
            getSupportLoaderManager().initLoader(0, null, this);
        }

        queryString = "";
    }

    /**
     * Method inflates the app bar menu with items.
     *
     * @param menu menu object
     * @return true
     */
    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        // Inflate the menu; this adds items to the action bar if it is present.
        getMenuInflater().inflate(R.menu.menu_main, menu);
        return true;
    }

    /**
     * Method does something upon app bar menu item selection.
     *
     * @param item app bar menu item
     * @return true (unless something bad happened)
     */

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        switch (item.getItemId()) {
            case R.id.action_settings:
                // User chose the "Settings" item, show the app settings UI...
                Intent intent = new Intent(this, SettingsActivity.class);
                startActivity(intent);
                return true;
            default:
                // If we got here, the user's action was not recognized.
                // Invoke the superclass to handle it.
                return super.onOptionsItemSelected(item);
        }
    }

    /**
     * Method begins Google Cloud endpoint query process upon button click.
     *
     * @param view view component
     */
    public void fetch(View view) {

        // Get the query string.
        queryString = editTextQueryString.getText().toString();

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
        if (networkInfo != null && networkInfo.isConnected()) {

            // Refactored to user AsyncTaskLoader via PlayerLoader.java
            Bundle queryBundle = new Bundle();
            queryBundle.putString("queryString", queryString);
            getSupportLoaderManager().restartLoader(0, queryBundle, this);

            // Indicate to user query is in process.
            textViewSearchResults.setText("");
            textViewSearchResults.setText(R.string.loading_in_process);
        } else {
            // There is no available connection.
            textViewSearchResults.setText("");
            textViewSearchResults.setText(R.string.no_connection);
        }
    }

    /**
     * Method called when load is instantiated.
     *
     * @param i integer value
     * @param bundle contains data in key-value pairs
     * @return query results
     */
    @NonNull
    @Override
    public Loader<String> onCreateLoader(int i, @Nullable Bundle bundle) {
        return new PlayerLoader(this, Objects.requireNonNull(bundle).getString("queryString"));
    }

    /**
     * Method called when loader task is finished.  Add code to update UI with results.
     *
     * @param loader Loader object.
     * @param s a String.
     */
    @SuppressLint("SetTextI18n")
    @Override
    public void onLoadFinished(@NonNull Loader<String> loader, String s) {

        // If string s is empty, then connection failed.
        if (s.contains("Connection failed!")) {
            textViewSearchResults.setText("");
            textViewSearchResults.setText(R.string.connection_failed);
            Toast.makeText(this, R.string.connection_failed, Toast.LENGTH_SHORT).show();
            return;
        }
        if (s.length() == 0) {
            textViewSearchResults.setText("");
            textViewSearchResults.setText(R.string.no_results_found);
            Toast.makeText(this, R.string.no_results_found, Toast.LENGTH_SHORT).show();
            return;
        }

        // obtain the JSON array of results items
        try {
            JSONObject jsonObject = new JSONObject(s);
            JSONArray itemsArray;

            // Condition to handle getting the player list
            if (queryString.length() == 0) {
                itemsArray = jsonObject.getJSONArray("items");

                Log.e(LOG_TAG, "Length of itemsArray: " + itemsArray.length());

                //Iterate through the results
                for (int i = 0; i < itemsArray.length(); i++) {
                    JSONObject player = itemsArray.getJSONObject(i); //Get the current item
                    String id = "id";
                    String email = "email";
                    String name = "default";

                    try {
                        id = player.getString("id");
                        email = player.getString("emailAddress");
                        name = player.getString("name");
                    } catch (Exception e) {
                        e.printStackTrace();
                    }

                    //If information field requested exists, update the TextViews and return
                    if (id != null || email != null || name != null) {
                        textViewSearchResults.append("\n\nid: " + id + "\n" + "email: " + email + "\n" + "name: " + name + "\n");
                    } else {
                        textViewSearchResults.append("\n\nFailure to retrieve any information for this particular player!\n");
                    }
                }
                return;
            }
            // Condition to get a specific player in list
            else {
                String id = "no id found";
                String email = "no email found";
                String name = "no name found";

                try {
                    id = jsonObject.getString("id");
                    email = jsonObject.getString("emailAddress");
                    name = jsonObject.getString("name");
                } catch (Exception e) {
                    e.printStackTrace();
                }

                //If information field requested exists, update the TextViews and return
                if (id != null || email != null || name != null) {
                    textViewSearchResults.append("\n\nid: " + id + "\n" + "email: " + email + "\n" + "name: " + name + "\n");
                    return;
                }
            }

            textViewSearchResults.setText("");
            textViewSearchResults.setText(R.string.display_failure);
            Toast.makeText(this, R.string.display_failure, Toast.LENGTH_SHORT).show();

        } catch (Exception ex) {

            textViewSearchResults.setText("");
            textViewSearchResults.setText(R.string.json_failure);
            Toast.makeText(this, R.string.json_failure, Toast.LENGTH_SHORT).show();
            ex.printStackTrace();

        } finally {
            Log.e(LOG_TAG, "Finished query process!");
        }
    }

    /**
     * Method called to clean up any remaining resources.
     *
     * @param loader Loader object.
     */
    @Override
    public void onLoaderReset(@NonNull Loader<String> loader) {

    }
}
