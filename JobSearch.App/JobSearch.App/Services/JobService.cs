using JobSearch.Domain.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace JobSearch.App.Services
{
    public class JobService : Service
    {
        public async Task<IEnumerable<Job>> GetJobs(string word, string cityState, int numberOfPage = 1)
        {
            HttpResponseMessage response = await _client.GetAsync($"{BaseApiUrl}/api/jobs?word={word}&cityState={cityState}&numberOfPage={numberOfPage}");

            List<Job> jobs = null;

            if (response.IsSuccessStatusCode)
            {
                jobs = await response.Content.ReadAsAsync<List<Job>>();
            }

            return jobs;
        }

        public async Task<Job> GetJob(int id)
        {
            HttpResponseMessage response = await _client.GetAsync($"{BaseApiUrl}/api/jobs/{id}");

            Job job = null;

            if (response.IsSuccessStatusCode)
            {
                job = await response.Content.ReadAsAsync<Job>();
            }

            return job;
        }

        public async Task<Job> AddJob(Job job)
        {
            HttpResponseMessage response = await _client.PostAsJsonAsync($"{BaseApiUrl}/api/jobs", job);

            if (response.IsSuccessStatusCode)
            {
                job = await response.Content.ReadAsAsync<Job>();
            }
            else
            {
                return null;
            }

            return job;
        }
    }
}
