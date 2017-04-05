/*
 * Jamal D. Scott
 * Arthur Byra
 * Tolls VES
 * Image Script
 * Computer Aid CAIHDC
 * 4/4/2017
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;

namespace VES_Image_Manipulation
{
    class Program
    {
        static void Main(string[] args)
        {
            //Get the current directory that the program is executing in.
            String directory = Directory.GetCurrentDirectory();
            directory += @"\Images";

            //Set a list of all images in the Image folder.
            String[] files = System.IO.Directory.GetFiles(directory, "*", System.IO.SearchOption.TopDirectoryOnly);
            //Array to hold the images.
            Bitmap[] image;
            Bitmap temp;
            int blurAmount;

            //Outer loop increments through each file path in the list.
            for (int i = 0; i < files.Length; i++)
            {
                //Creates a new instance of the array to clear out the values that were left in the image array.
                image = new Bitmap[5];

                //Creates a new instance of random that will help scramble the images
                Random r = new Random();
                bool[] visited = new bool[5];

                //Creates a new sub-directory based on the image that we're looking at.
                String newDir = directory + @"\Image" + i;
                System.IO.Directory.CreateDirectory(newDir);

                for (int j = 0; j < 5; j++)
                {
                    /*
                     * For simplicity, set the blur ammount to an already incrementing value.
                     * because each new image needs to be blurrier
                     */
                    blurAmount = j;

                    if (j == 0)
                    {
                        image[j] = new Bitmap(files[i]);
                        temp = new Bitmap(image[j]);
                    }
                    else
                        //Used the previously blurred image to make the next one blurrier.
                        temp = new Bitmap(image[j - 1]);

                    //Calls the method to blur our image.
                    temp = blur(temp, blurAmount);

                    //Stores our newly blurred image.
                    image[j] = temp;

                    //inefficiently finds a number between 0 and 4 inclusively that hasn't been already used to assign as the name of the new image
                    int rand = r.Next(0, 5);

                    //Shuffles the ordering of the elements of the array.
                    while (visited[rand] == true)
                        rand = r.Next(0, 5);

                    visited[rand] = true;

                    //Writes the image to a jpg file in its directory.
                    image[j].Save(newDir + "/img" + rand + ".jpg");
                    Console.WriteLine("\nWrote: " + newDir + "/img" + rand + ".jpg");
                }
            }

            Console.WriteLine("\nCompleted! All files have been saved to the directory: \n" + directory);
            Console.WriteLine("\nPress any key to exit.");
            Console.ReadKey();
        }

        //Function to blur an image by pixel manipulation.
        static Bitmap blur(Bitmap img, int amount)
        {
            Bitmap buffBitmap = img;

            for (int blr = 0; blr <= amount; blr++)
            {
                for (int x = blr; x < img.Width - blr; x++)
                {
                    for (int y = blr; y < img.Height - blr; y++)
                    {
                        //Gets the new pixel color range of the image (blurring an image "stretches" the image's pixels.)
                        Color prevX = img.GetPixel(x - blr, y);
                        Color nextX = img.GetPixel(x + blr, y);
                        Color prevY = img.GetPixel(x, y - blr);
                        Color nextY = img.GetPixel(x, y + blr);

                        //Calculates the new "stretched" rgb values to set the pixels
                        int avgR = (int)((prevX.R + nextX.R + prevY.R + nextY.R) / 4);
                        int avgG = (int)((prevX.G + nextX.G + prevY.G + nextY.G) / 4);
                        int avgB = (int)((prevX.B + nextX.B + prevY.B + nextY.B) / 4);

                        //Sets the pixel at location (x,y) to our new calculated value.
                        buffBitmap.SetPixel(x, y, Color.FromArgb(avgR, avgG, avgB));
                    }
                }
            }
            //Send back our blurred image to write out.
            img = buffBitmap;
            return img;
        }
    }
}
