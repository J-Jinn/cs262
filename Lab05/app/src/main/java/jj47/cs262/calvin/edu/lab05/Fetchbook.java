package jj47.cs262.calvin.edu.lab05;

import android.os.AsyncTask;
import android.util.Log;
import android.widget.TextView;

import org.json.JSONArray;
import org.json.JSONObject;

public class Fetchbook extends AsyncTask<String,Void,String>{

    private static final String LOG_TAG = Fetchbook.class.getSimpleName();

    private TextView mTitleText;
    private TextView mAuthorText;

    /**
     * Constructor.
     *
     * @param mTitleText init
     * @param mAuthorText init
     */
    public Fetchbook(TextView mTitleText, TextView mAuthorText) {
        this.mTitleText = mTitleText;
        this.mAuthorText = mAuthorText;
    }

    /**
     * Call NetworkUtils class to perform the query
     *
     * @param params string array
     * @return results of query
     */
    @Override
    protected String doInBackground(String... params) {

        return NetworkUtils.getBookInfo(params[0]);
    }

    /**
     * Method parses the JSON results upon completion of task.
     *
     * @param s JSON object
     */
    @Override
    protected void onPostExecute(String s) {
        super.onPostExecute(s);

        // obtain the JSON array of results items
        try {
            JSONObject jsonObject = new JSONObject(s);
            JSONArray itemsArray = jsonObject.getJSONArray("items");

            //Iterate through the results
            for (int i = 0; i < itemsArray.length(); i++) {
                JSONObject book = itemsArray.getJSONObject(i); //Get the current item
                String title = null;
                String authors = null;
                JSONObject volumeInfo = book.getJSONObject("volumeInfo");

                try {
                    title = volumeInfo.getString("title");
                    authors = volumeInfo.getString("authors");
                } catch (Exception e) {
                    e.printStackTrace();
                }

                //If both a title and author exist, update the TextViews and return
                if (title != null && authors != null) {
                    mTitleText.setText(title);
                    mAuthorText.setText(authors);
                    return;
                }
            }

            mTitleText.setText(R.string.no_results_found);
            mAuthorText.setText("");

        } catch (Exception ex){

            mTitleText.setText(R.string.no_results_found);
            mAuthorText.setText("");
            ex.printStackTrace();

        } finally{
            Log.e(LOG_TAG,"Finished");
            return;
        }
    }
}