// Importações / Dependências
import { BrowserRouter, Routes, Route } from 'react-router-dom';
/* Importação das suas 3 páginas */
import { LoginPage } from './pages/LoginPage';
import { RegisterPage } from './pages/RegisterPage';
import { QuizPage } from './pages/QuizPage';
import { ProfileDashboard } from './pages/ProfileDashboard';
import { FeedPage } from './pages/FeedPage';
import { ProtectedRoute } from './components/ProtectedRoute'; /* Vigia de Rotas */


function App() {
  return (
    <BrowserRouter>
      <Routes>
        {/* Rotas Públicas */}
        <Route path="/" element={<LoginPage />} />
        <Route path="/cadastro" element={<RegisterPage />} />

        {/* Rotas Protegidas (Envolvidas pelo ProtectedRoute) */}
        <Route 
          path="/onboarding" 
          element={
            <ProtectedRoute>
              <QuizPage />
            </ProtectedRoute>
          } 
        />

        <Route 
          path="/perfil" 
          element={
            <ProtectedRoute>
              <ProfileDashboard />
            </ProtectedRoute>
          } 
        />

        <Route 
          path="/feed" 
          element={
            <ProtectedRoute>
              <FeedPage />
            </ProtectedRoute>
          } 
        />
      </Routes>
    </BrowserRouter>
  );
}

export default App;