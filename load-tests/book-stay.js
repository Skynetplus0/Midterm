// JavaScript source code
import http from 'k6/http';
import { check, sleep } from 'k6';

export const options = {
    scenarios: {
        normal_load: {
            executor: 'constant-vus',
            vus: 20,
            duration: '30s',
        },
        peak_load: {
            executor: 'constant-vus',
            vus: 50,
            duration: '30s',
            startTime: '35s',
        },
        stress_load: {
            executor: 'constant-vus',
            vus: 100,
            duration: '30s',
            startTime: '70s',
        },
    },
    thresholds: {
        http_req_failed: ['rate<0.20'],
        http_req_duration: ['p(95)<2000'],
    },
};

const BASE_URL = 'https://localhost:7252';

// Buraya Swagger login'den aldığın guest tokenı yapıştır
const GUEST_TOKEN = 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxIiwiZW1haWwiOiJob3N0QHRlc3QuY29tIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZWlkZW50aWZpZXIiOiIxIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZSI6IkRlbW8gSG9zdCIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL2VtYWlsYWRkcmVzcyI6Imhvc3RAdGVzdC5jb20iLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJIb3N0IiwianRpIjoiODQ0YTU5YzItY2ZlYy00MDZlLWI3NDItODE2YWIwMTQwOWQzIiwiZXhwIjoxNzc0NjkwNDY4LCJpc3MiOiJTdGF5Qm9va2luZ0FwaSIsImF1ZCI6IlN0YXlCb29raW5nQXBpVXNlcnMifQ.IDym5ejWsh3-56tGPLNSOpf5LE-8LMocjIp4m_SCyFs';

export default function () {
    const dayOffset = (__VU + __ITER) % 20;
    const fromDay = 10 + dayOffset;
    const toDay = fromDay + 2;

    const payload = JSON.stringify({
        listingId: 1,
        fromDate: `2026-05-${String(fromDay).padStart(2, '0')}`,
        toDate: `2026-05-${String(toDay).padStart(2, '0')}`,
        peopleNames: [`Guest-${__VU}-${__ITER}`],
    });

    const params = {
        headers: {
            'Content-Type': 'application/json',
            Authorization: `Bearer ${GUEST_TOKEN}`,
        },
    };

    const res = http.post(`${BASE_URL}/api/v1/guests/bookings`, payload, params);

    check(res, {
        'booking status is 200 or 400': (r) => r.status === 200 || r.status === 400,
    });

    sleep(1);
}