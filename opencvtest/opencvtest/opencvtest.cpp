// opencvtest.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

#include <opencv2/opencv.hpp>
#include <iostream>
#include <fstream>

int main()
{
	std::string line;
	std::ifstream myfile("C:/Users/User/Documents/GitHub/AR-Oktatasi-Segedeszkoz-Meta-2-Hasznalataval/out.txt");
	float inputScreenpoints[8];
	int cv = 0;
	if (myfile.is_open())
	{
		while (getline(myfile, line))
		{
			std::cout << line << '\n';
			inputScreenpoints[cv] = std::stof(line);
			cv++;
		}
		myfile.close();
	}

	float fx = 630.9;
	float fy = 630.7;
	float cx = 636.5;
	float cy = 337.1;
	cv::Mat intrinsic = cv::Mat::zeros(3, 3, CV_32FC1);
	intrinsic.at<float>(0, 0) = fx;
	intrinsic.at<float>(1, 1) = fy;
	intrinsic.at<float>(0, 2) = cx;
	intrinsic.at<float>(1, 2) = cy;
	intrinsic.at<float>(2, 2) = 1;
	std::cout << intrinsic << std::endl;
	cv::Mat objpoints = cv::Mat::zeros(4, 3, CV_32FC1);
	objpoints.at<float>(1, 1) = 32;
	objpoints.at<float>(2, 0) = 38;
	objpoints.at<float>(3, 0) = 38;
	objpoints.at<float>(3, 1) = 32;
	std::cout << objpoints << std::endl;

	cv::Mat screenpoints = cv::Mat::zeros(4, 2, CV_32FC1);
	screenpoints.at<float>(0, 0) = inputScreenpoints[0];
	screenpoints.at<float>(0, 1) = inputScreenpoints[1];
	screenpoints.at<float>(1, 0) = inputScreenpoints[2];
	screenpoints.at<float>(1, 1) = inputScreenpoints[3];
	screenpoints.at<float>(3, 0) = inputScreenpoints[4];
	screenpoints.at<float>(3, 1) = inputScreenpoints[5];
	screenpoints.at<float>(2, 0) = inputScreenpoints[6];
	screenpoints.at<float>(2, 1) = inputScreenpoints[7];
	std::cout << screenpoints << std::endl;


	cv::Mat distcoeff = cv::Mat::zeros(4, 1, CV_32FC1);
	cv::Mat rvec, tvec;
	std::cout << "object point size:" << objpoints.size() << std::endl;
	std::cout << "screenpoints size:" << screenpoints.size() << std::endl;
	std::cout << "intrinsic size:" << intrinsic.size() << std::endl;
	std::cout << "distcoeff size:" << distcoeff.size() << std::endl;
	cv::solvePnP(objpoints, screenpoints, intrinsic, distcoeff, rvec, tvec);
	cv::Mat rodr;
	cv::Rodrigues(rvec, rodr);
	std::cout << rodr << std::endl;
	std::cout << tvec << std::endl;

	std::ofstream outfile("C:/Users/User/Documents/GitHub/AR-Oktatasi-Segedeszkoz-Meta-2-Hasznalataval/solvedout.txt");
	if (outfile.is_open())
	{
		outfile << rodr.at<double>(0,0) << "\n";
		outfile << rodr.at<double>(0,1) << "\n";
		outfile << rodr.at<double>(0, 2) << "\n";
		outfile << rodr.at<double>(1, 0) << "\n";
		outfile << rodr.at<double>(1, 1) << "\n";
		outfile << rodr.at<double>(1, 2) << "\n";
		outfile << rodr.at<double>(2, 0) << "\n";
		outfile << rodr.at<double>(2, 1) << "\n";
		outfile << rodr.at<double>(2, 2) << "\n";
		outfile << tvec.at<double>(0) << "\n";
		outfile << tvec.at<double>(1) << "\n";
		outfile << tvec.at<double>(2) << "\n";
		outfile.close();
	}
	else std::cout << "Unable to open file";
}

// Run program: Ctrl + F5 or Debug > Start Without Debugging menu
// Debug program: F5 or Debug > Start Debugging menu

// Tips for Getting Started: 
//   1. Use the Solution Explorer window to add/manage files
//   2. Use the Team Explorer window to connect to source control
//   3. Use the Output window to see build output and other messages
//   4. Use the Error List window to view errors
//   5. Go to Project > Add New Item to create new code files, or Project > Add Existing Item to add existing code files to the project
//   6. In the future, to open this project again, go to File > Open > Project and select the .sln file
